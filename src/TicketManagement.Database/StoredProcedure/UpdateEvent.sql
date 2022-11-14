CREATE PROCEDURE dbo.UpdateEvent
	@id INT,
	@name NVARCHAR(120),
	@description NVARCHAR(MAX),
	@layoutId INT,
	@dateTimeStart DATETIME2,
	@dateTimeEnd DATETIME2,
	@areaPrice DECIMAL(18, 0),
	@seatState INT,
	@url NVARCHAR(2000)

AS
-- when changing the Layout, you need to replace all existing Area and Seat,
-- deleting the existing ones and adding new ones
IF NOT EXISTS (SELECT LayoutId FROM Event WHERE Event.Id = @id AND Event.LayoutId = @layoutId)
BEGIN
	-- delete old EventSeats, if LayoutId change
	BEGIN
	DELETE FROM EventSeat 
	WHERE EventAreaId IN (SELECT Id FROM EventArea WHERE EventId = @id)
	END
	-- delete old EventArea, if LayoutId change
	BEGIN
	DELETE FROM EventArea
	WHERE EventId = @id
	END

	-- update concreate event
	BEGIN
	UPDATE Event
	SET Name = @name,
		Description = @description,
		LayoutId = @layoutId,
		DateTimeStart = @dateTimeStart,
		DateTimeEnd = @dateTimeEnd,
		URL = @url
	WHERE Id = @id
	END

	-- add new EventArea for new layoutId
	BEGIN
	INSERT INTO EventArea(EventId, Description, CoordX, CoordY, Price)
		SELECT (SELECT Id FROM Event WHERE Id = @id), Area.Description, CoordX, CoordY, @areaPrice
		FROM Area
		WHERE Area.LayoutId = @layoutId
	END

	-- add new EventSeats for new layoutId
	BEGIN
	INSERT INTO EventSeat(EventAreaId, Row, Number, State)
		SELECT (SELECT SCOPE_IDENTITY()), Seat.Row, Seat.Number, @seatState
		FROM Seat, Area
		WHERE Seat.AreaId = Area.Id AND Area.LayoutId = @layoutId
	END
END

-- if the Layout does not change, it is enough to change the rest of the Event fields
ELSE
BEGIN
	-- update concreate event
	BEGIN
	UPDATE Event
	SET Name = @name,
		Description = @description,
		LayoutId = @layoutId,
		DateTimeStart = @dateTimeStart,
		DateTimeEnd = @dateTimeEnd,
		URL = @url
	WHERE Id = @id
	END
END

