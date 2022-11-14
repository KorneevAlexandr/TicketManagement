--- Roles
insert into dbo.Roles
values('Admin'),
('User'),
('ModeratorVenue'),
('ModeratorEvent')

--- Users
insert into dbo.Users
values ('Admin', 'Admin', 20, 'En', '12-10-21 12:32:10 +01:00', 'admin@mail.ru', 'Admin', 'Admin', 0),
('Alex', 'Korneev', 20, 'En', '12-10-21 12:32:10 +01:00', 'event@mail.ru', 'Event', 'Alex', 0),
('Maks', 'Kasyan', 20, 'En', '12-10-21 12:32:10 +01:00', 'venue@mail.ru', 'Venue', 'Maks', 0),
('Igor', 'Maksimov', 20, 'En', '12-10-21 12:32:10 +01:00', 'user@mail.ru', 'User', 'Igor', 0),
('Irina', 'Ivanova', 20, 'En', '12-10-21 12:32:10 +01:00', 'user@mail.ru', 'UserX', 'Irina', 0)

-- UserRoles
insert into dbo.UserRoles
values (1, 1),
(2, 4),
(3, 3),
(4, 2),
(5, 2)

--- Venue
insert into dbo.Venue
values ('Well dance', 'Homel', '+375291234567', 'Dance')

--- Layout
insert into dbo.Layout
values (1, 'Second layout', 'Hol 2'),
(1, 'First layout', 'Hol 1')

--- Area
insert into dbo.Area
values (1, 'Middle quality', 1, 1),
(1, 'High quality', 2, 2),
(2, 'Middle quality', 1, 1)

--- Seat
insert into dbo.Seat
values (1, 1, 1),
(1, 2, 1),
(1, 3, 1),
(1, 4, 1),
(1, 5, 1),
(1, 6, 1),
(2, 1, 1),
(2, 2, 1),
(3, 1, 1),
(3, 1, 2),
(3, 1, 3),
(3, 2, 1),
(3, 2, 2),
(3, 2, 3),
(3, 3, 1),
(3, 3, 2),
(3, 3, 3)

--- Event
execute dbo.InsertEvent
	@name = 'Konstantinopol', 
	@description = 'Very well event!',
	@layoutId = 1,
	@dateTimeStart = '01/02/2022',
	@dateTimeEnd = '02/02/2022',
	@areaPrice = 100,
	@seatState = 1,
	@url = 'https://media-cdn.tripadvisor.com/media/photo-s/17/b5/b6/68/caption.jpg'

execute dbo.InsertEvent
	@name = N'Мадагаскар', 
	@description = N'Отличный мультик, на который нужно идти всем!',
	@layoutId = 2,
	@dateTimeStart = '03/02/2022',
	@dateTimeEnd = '04/02/2022',
	@areaPrice = 80,
	@seatState = 1,
	@url = N'https://upload.wikimedia.org/wikipedia/ru/2/28/Madagascar_film.jpg'
