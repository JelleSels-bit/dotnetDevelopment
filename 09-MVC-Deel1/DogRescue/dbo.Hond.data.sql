/*
--------------------------------------------------
	Voer "eerst" de commands uit via console:

	Add-Migration MIG0
	Update-Database
--------------------------------------------------
     Voer daarna dit bestand pas uit:
*/

SET IDENTITY_INSERT [dbo].[Hond] ON
INSERT INTO [dbo].[Hond] ([HondId], [Naam], [Geboortedatum], [Geslacht], [Opmerkingen]) VALUES (1, N'Oscar', N'2016-03-21 00:00:00', 1, N'Deze hond werd gered op het eiland Tasmanië.')
INSERT INTO [dbo].[Hond] ([HondId], [Naam], [Geboortedatum], [Geslacht], [Opmerkingen]) VALUES (2, N'Dezzer', N'2012-12-04 00:00:00', 0, N'Deze hond werd gered door Team 6.')
INSERT INTO [dbo].[Hond] ([HondId], [Naam], [Geboortedatum], [Geslacht], [Opmerkingen]) VALUES (3, N'Snuffer', N'2020-07-08 00:00:00', 1, N'Onbekende gegevens.')
SET IDENTITY_INSERT [dbo].[Hond] OFF