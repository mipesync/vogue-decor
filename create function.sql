CREATE OR REPLACE FUNCTION update_search_vector() RETURNS trigger AS $$
BEGIN
  NEW."SearchVector" := to_tsvector('russian'::regconfig, coalesce(NEW."Name", '') || ' ' || (
	SELECT COALESCE(pt."Name", '') FROM "ProductTypes" AS pt WHERE pt."Id" = NEW."ProductType") || ' ' || (
		SELECT CASE WHEN array_length(NEW."PictureMaterial", 1) IS NULL THEN '' ELSE COALESCE(concat(string_agg(c."Name", ' '), ' рамка'), '') END FROM "Materials" AS c WHERE c."Id" = ANY(COALESCE(NEW."PictureMaterial", '{}')))
 || ' ' || (
			SELECT COALESCE(string_agg(c."Name" || ' (' || c."EngName" || ')', ' '), '') FROM "Colors" AS c WHERE c."Id" = ANY(COALESCE(NEW."Colors", '{}'))) || ' ' || (
				SELECT COALESCE(string_agg(c."Name", ''), '') FROM "Styles" AS c WHERE c."Id" = ANY(COALESCE(NEW."Styles", '{}'))) || ' ' || (
					SELECT COALESCE(string_agg(c."Name", ''), '') FROM "Materials" AS c WHERE c."Id" = ANY(COALESCE(NEW."Materials", '{}'))) || ' ' || (
						SELECT COALESCE(string_agg(c."Name", ''), '') FROM "Categories" AS c WHERE c."Id" = ANY(COALESCE(NEW."Types", '{}'))) || ' ' || (
							SELECT COALESCE(string_agg(ct."Name", ''), '') FROM "ChandelierTypes" AS ct WHERE ct."Id" = ANY(COALESCE(NEW."ChandelierTypes", '{}'))));
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;
