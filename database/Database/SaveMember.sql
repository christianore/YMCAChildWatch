/*
	AUTHOR:		CHRISTIAN ORE
	DATE:		3/25/2018
	DESCRIPTION
	THIS STORED PROCEDURE WILL SAVE A MEMBER TO THE DATABASE OR ADD THEM IF THEY ARE NOT THERE.

*/

ALTER PROCEDURE p_member_save
	@member_id		VARCHAR(11),
	@first_name		VARCHAR(20),
	@last_name		VARCHAR(30),
	@barcode		VARCHAR(6),
	@pin			VARCHAR(4),
	@phone			VARCHAR(13),
	@active			BIT
AS

BEGIN
	
	DECLARE
		@memberCount INT 
	SELECT
		@memberCount = COUNT(member_id)
	FROM
		member WITH(NOLOCK)
	WHERE
		member_id = @member_id
	
	IF @memberCount = 0
		INSERT INTO member WITH(ROWLOCK) VALUES(@member_id, @first_name, @last_name, @barcode, @pin, @phone, @active)
	ELSE IF @memberCount = 1
		UPDATE member WITH(ROWLOCK)
		SET 
			member_fName = @first_name,
			member_lName = @last_name,
			barcode = @barcode,
			pin = @pin,
			phone = @phone,
			active = @active 
		WHERE
			member_id = @member_id

END
