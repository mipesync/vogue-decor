using AutoMapper;
using AutoMapper.QueryableExtensions;
using vogue_decor.Application.Common.Exceptions;
using vogue_decor.Application.DTOs.UserDTOs;
using vogue_decor.Application.DTOs.UserDTOs.ResponseDTOs;
using vogue_decor.Application.Interfaces;
using vogue_decor.Application.Interfaces.Repositories;
using vogue_decor.Domain;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace vogue_decor.Application.Repositories
{
    /// <summary>
    /// Репозиторий пользователя
    /// </summary>
    /// <inheritdoc/>
    public class UserRepository : IUserRepository
    {
        private readonly IDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public UserRepository(IDBContext dbContext, IMapper mapper, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task<GetUsersResponseDto> GetAll()
        {
            var users = await _dbContext.Users
                .AsNoTracking()
                .ProjectTo<UserShortResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var result = new GetUsersResponseDto
            {
                Users = users
            };

            return result;
        }

        public async Task<UserResponseDto> GetById(GetUserByIdDto dto)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user is null)
                throw new NotFoundException(user);

            var result = _mapper.Map<UserResponseDto>(user);

            return result;
        }

        public async Task<GetCartResponseDto> GetCart(GetUserByIdDto dto)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.ProductUsers)
                .ThenInclude(u => u.Product)
                .ThenInclude(u => u.Favourites)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user is null)
                throw new NotFoundException(user);
            
            var counts = GetCounts(dto.UserId);

            var result = ParseToCart(user.ProductUsers);
            result.CartCount = counts.cart;
            result.FavouritesCount = counts.favourites;

            return result;
        }

        public async Task ClearCart(Guid userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.ProductUsers)
                .FirstOrDefaultAsync(u => u.Id == userId, CancellationToken.None);
            
            if (user is null)
                throw new NotFoundException(user);
            
            user.ProductUsers.Clear();

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<GetCartResponseDto> GetFavourites(GetUserByIdDto dto)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Favourites)
                .ThenInclude(u => u.Product)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user is null)
                throw new NotFoundException(user);
            
            var counts = GetCounts(dto.UserId);

            var result = ParseToCart(user.Favourites);
            result.CartCount = counts.cart;
            result.FavouritesCount = counts.favourites;

            return result;
        }

        public async Task UpdateDetails(UpdateUserDetailsDto dto)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);
            
            if (user is null)
                throw new NotFoundException(user);

            user.Name = dto.Name;
            user.PhoneNumber = dto.PhoneNumber;

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task OrderPlace(OrderPlaceDto dto)
        {
            var user = await _dbContext.Users
                .Include(u => u.ProductUsers)
                .ThenInclude(u => u.Product)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user is null)
                throw new NotFoundException(user);

            var cart = ParseToCart(user.ProductUsers);

            var orderComposition = new StringBuilder();

            var iter = 0;

            var totalCost = 0m;

            foreach (var product in cart.Products)
            {
                iter++;

                totalCost += product.Price;

                orderComposition.Append(
                    "<tr>" +
                    $"<th>{iter}</th>" +
                    $"<th>{product.Id}</th>" +
                    $"<th>{product.Name}</th>" +
                    $"<th>{product.Count}шт.</th>" +
                    $"<th>{product.Price}р.</th>" +
                    "</tr>");
            }

            var message =
                "Состав заказа:<br/>" +
                "<table width=\"80%\">" +
                "" +
                "<th>Позиция</th>" +
                "<th>Id</th>" +
                "<th>Название</th>" +
                "<th>Кол-во</th>" +
                "<th>Стоимость</th>" +
                $"{orderComposition}" +
                "</table>" +
                "<table align=\"right\">" +
                "<th><b>Итого: </b></th>" +
                $"<th><b>{totalCost}р.</b></th>" +
                $"</table>";

            await _emailSender.SendEmailAsync("Новый заказ",
                "<b>Информация о пользователе:</b> <br/>" +
                "<ul>" +
                $"<b><li>Имя:</li></b> {dto.Name} <br/>" +
                $"<b><li>Адрес почты:</li></b> {user.Email} <br/>" +
                $"<b><li>Номер телефона:</li></b> {dto.PhoneNumber} <br/>" +
                "</ul>" +
                "<b>Комментарий к заказу:</b><br/>" +
                $"{dto.Comment}" +
                "<b><hr/></b><br/>" + message);

            await _emailSender.SendEmailAsync(user.Email, "Заказ успешно оформлен",
                $"{dto.Name}, спасибо за заказ! <br/>" + message +
                $"С уважением, администрация Butterfly Lightning Co.");

            foreach (var product in user.Products)
            {
                product.PurchasedCount++;
            }
            
            user.ProductUsers.Clear();
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task Feedback(FeedbackDto dto)
        {
            await _emailSender.SendEmailAsync(dto.Email, "Обращение к администрации",
                $"Уважаемый {dto.Name}, ваще обращение было зарегистрировано. " +
                $"Ожидайте ответ от администрации на эту почту в ближайшее время. " +
                $"С уважением, администрация Butterfly Lightning Co.");

            await _emailSender.SendEmailAsync("Новое обращение",
                "<b>Информация о пользователе:</b> <br/>" +
                "<ul>" +
                $"<b><li>Имя:</li></b> {dto.Name} <br/>" +
                $"<b><li>Адрес почты:</li></b> {dto.Email} <br/>" +
                "<b><hr/></b><br/>" +
                $"Текст обращения: \n" +
                $"{dto.Text}");
        }

        private GetCartResponseDto ParseToCart(List<ProductUser> productUsers)
        {
            var result = new GetCartResponseDto();

            foreach (var productUser in productUsers)
            {
                if (result.Products.Any(u => u.Id == productUser.ProductId))
                    continue;

                var count = productUsers.Count(u => u.ProductId == productUser.ProductId);

                var mappedProduct = _mapper.Map<CartResponseDto>(productUser.Product, opt =>
                {
                    opt.Items["userId"] = productUser.UserId;
                });

                mappedProduct.Count = count;
                mappedProduct.Price = productUser.Product.Price;

                result.Products.Add(mappedProduct);
            }

            return result;
        }

        private GetCartResponseDto ParseToCart(List<Favourite> favourites)
        {
            var result = new GetCartResponseDto();

            foreach (var productUser in favourites)
            {
                if (result.Products.Any(u => u.Id == productUser.ProductId))
                    continue;

                var count = favourites.Count(u => u.ProductId == productUser.ProductId);

                var mappedProduct = _mapper.Map<CartResponseDto>(productUser.Product, opt =>
                {
                    opt.Items["userId"] = productUser.UserId;
                });

                mappedProduct.Count = count;
                mappedProduct.Price = productUser.Product.Price;

                result.Products.Add(mappedProduct);
            }

            return result;
        }

        private (int favourites, int cart) GetCounts(Guid userId)
        {
            var favouritesCount = _dbContext.Favourites.Count(u => u.UserId == userId);
            var cartCount = _dbContext.ProductUsers.Count(u => u.UserId == userId);

            return (favouritesCount, cartCount);
        }
    }
}
