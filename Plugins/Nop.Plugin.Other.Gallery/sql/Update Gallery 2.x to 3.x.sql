ALTER TABLE [dbo].[Gallery]
	ADD SimpleGallery  BIT not null DEFAULT 1
		GO
ALTER TABLE [dbo].[Gallery]
	ADD [FlickrGallery]  BIT not null DEFAULT 0
		GO
ALTER TABLE [dbo].[Gallery]
	ADD [PicasaGallery]  BIT not null DEFAULT 0
		GO
ALTER TABLE [dbo].[Gallery]
	ADD[VideoGallery]  BIT not null DEFAULT 0
		GO
ALTER TABLE [dbo].[Gallery]
	ADD [FlickrKeyword] [nvarchar](400)
		GO
ALTER TABLE [dbo].[Gallery]
	ADD [PicasaKeyword] [nvarchar](400)
		GO
ALTER TABLE [dbo].[Gallery]
	ADD [VideoLink] [nvarchar](400)
		GO
ALTER TABLE [dbo].[ImageGallery]
	ADD [CreatedOnUtc] [datetime]
		GO
ALTER TABLE [dbo].[ImageGallery]
	ADD [UpdatedOnUtc] [datetime]
		GO
	ALTER TABLE [dbo].[ImageGallery]
	ADD [ShareImage] BIT not null DEFAULT 0
		