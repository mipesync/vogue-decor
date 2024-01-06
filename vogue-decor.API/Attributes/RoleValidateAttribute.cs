﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using vogue_decor.Domain.Enums;

namespace vogue_decor.Attributes
{
    public class RoleValidateAttribute : Attribute, IActionFilter
    {
        private readonly Roles Role;

        public RoleValidateAttribute(Roles role)
        {
            Role = role;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var token = context.HttpContext.Request.Headers["Authorization"]
                    .FirstOrDefault()!
                    .Replace("Bearer", "")
                    .Trim();

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);

                if (jwtSecurityToken.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Role)!.Value != Role.ToString())
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new ForbidResult();
                }
            }
            catch(Exception ex) {}
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Нет реализации
        }
    }
}
