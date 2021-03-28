CREATE PROCEDURE ImgsAdd
@ImgRoute text
AS
INSERT INTO dbo.ImageRoutes(ImgRoute)
VALUES (@ImgRoute)
