UPDATE "Products" AS p SET "SearchVector" = to_tsvector('russian'::regconfig, p."Name" || ' ' || (
	SELECT COALESCE(pt."Name", '') FROM "ProductTypes" AS pt WHERE pt."Id" = p."ProductType") || ' ' || (
		SELECT CASE WHEN array_length(p."PictureMaterial", 1) IS NULL THEN '' ELSE COALESCE(concat(string_agg(c."Name", ' '), ' рамка'), '') END FROM "Materials" AS c WHERE c."Id" = ANY(COALESCE(p."PictureMaterial", '{}')))
 || ' ' || (
			SELECT COALESCE(string_agg(c."Name" || ' (' || c."EngName" || ')', ' '), '') FROM "Colors" AS c WHERE c."Id" = ANY(COALESCE(p."Colors", '{}'))) || ' ' || (
				SELECT COALESCE(string_agg(c."Name", ''), '') FROM "Styles" AS c WHERE c."Id" = ANY(COALESCE(p."Styles", '{}'))) || ' ' || (
					SELECT COALESCE(string_agg(c."Name", ''), '') FROM "Materials" AS c WHERE c."Id" = ANY(COALESCE(p."Materials", '{}'))) || ' ' || (
						SELECT COALESCE(string_agg(c."Name", ''), '') FROM "Categories" AS c WHERE c."Id" = ANY(COALESCE(p."Types", '{}'))) || ' ' || (
							SELECT COALESCE(string_agg(ct."Name", ''), '') FROM "ChandelierTypes" AS ct WHERE ct."Id" = ANY(COALESCE(p."ChandelierTypes", '{}'))));