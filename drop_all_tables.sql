USE IdettaTestDB;

DECLARE @sql NVARCHAR(max)=''

SELECT @sql += ' Drop table ' + QUOTENAME(name) + '; '
FROM   sys.tables
ORDER BY create_date desc;

Exec Sp_executesql @sql;