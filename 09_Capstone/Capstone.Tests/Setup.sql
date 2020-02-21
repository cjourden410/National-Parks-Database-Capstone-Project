Delete From site
delete from reservation
delete from park
delete from campground

insert into park
(name, location, establish_date, area, visitors, description)
values('Twilight Park', 'Ohio', '1999-12-25', 9001, 2000000, 'A beautiful park that has a great sunset, perfect for eating ice cream!')

Declare @newPark_Id int = (SELECT @@IDENTITY);
