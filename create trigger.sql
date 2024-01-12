CREATE TRIGGER update_search_vector BEFORE INSERT OR UPDATE ON "Products"
FOR EACH ROW EXECUTE PROCEDURE update_search_vector();
