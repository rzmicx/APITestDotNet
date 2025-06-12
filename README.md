berikut adalah bagian untuk API

untuk development jangan lupa ubah file appsetting.json untuk settingan redis dan database 
jika code ini mau dijalankan langsung bisa di run di visual studio
jika ingin di deploy:
1, Publish project ini as a folder 
2, buka aplikasi IIS
3, create new application
4, Pilih letak dimana project ini di publish
5. run application

jangan lupa jalankan script database:
CREATE DATABASE TestDotNet
USE TestDotNet
GO

/****** Object:  Table [dbo].[msuser]    Script Date: 12/06/2025 21:54:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[msuser](
	[id] [INT] IDENTITY PRIMARY KEY,
	[username] [VARCHAR](50) NULL,
	[passcode] [VARBINARY](MAX) NULL,
	[active] [BIT] NULL 
 
)
 
CREATE TABLE Product
     (
            Id  int IDENTITY PRIMARY KEY,
            Name   varchar(500),
            Description  varchar(MAX),
            Price  decimal(17,2),
            CreatedAt  datetime,
		    CreateBy varchar(500)
    )


	--SELECT * FROM dbo.msuser
	--SELECT * FROM dbo.Product

create  PROC sp_GridData   @type varchar(50), @orderby varchar(50)
AS
DECLARE @sql VARCHAR(MAX);
SET @sql = 'SELECT * FROM dbo.Product '
DECLARE @where VARCHAR(500);
SET @where='';
IF @orderby <>''
BEGIN
    SET @where = ' where ' + @type +' = ' +@orderby
END
EXEC (@sql + @where +' ORDER BY CreatedAt desc  ')
 

    --EXEC sp_griddata 'id', '1'

