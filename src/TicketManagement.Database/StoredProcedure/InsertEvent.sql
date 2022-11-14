CREATE PROCEDURE dbo.InsertEvent
	@name NVARCHAR(120),
	@description NVARCHAR(MAX),
	@layoutId INT,
	@dateTimeStart DATETIME2,
	@dateTimeEnd DATETIME2,
	@areaPrice decimal(18, 0),
	@seatState int,
	@url nvarchar(2000)

AS

-- insert info about event
BEGIN
INSERT INTO Event(Name, Description, LayoutId, DateTimeStart, DateTimeEnd, URL)
VALUES(@name, @description, @layoutId, @dateTimeStart, @dateTimeEnd, @url)
END

DECLARE @EventId INT;
SET @EventId = (SELECT SCOPE_IDENTITY());

-- insert EventArea using places Area
BEGIN
INSERT INTO EventArea(EventId, Description, CoordX, CoordY, Price)
	SELECT @EventId, Area.Description, CoordX, CoordY, @areaPrice
	FROM Area
	WHERE Area.LayoutId = @layoutId
END

-- insert EventSeat using places Seat
BEGIN
INSERT INTO EventSeat(EventAreaId, Row, Number, State)
	SELECT (SELECT MAX(EventArea.Id) 
			FROM EventArea, Area, Event
			WHERE EventArea.CoordX = Area.CoordX 
			AND EventArea.CoordY = Area.CoordY 
			AND EventArea.EventId = @EventId 
			AND Event.LayoutId = @layoutId
			AND Area.LayoutId = @layoutId
			AND Seat.AreaId = Area.Id),
			
			Seat.Row, Seat.Number, @seatState

	FROM Seat, Area
	WHERE Seat.AreaId = Area.Id AND Area.LayoutId = @layoutId
END