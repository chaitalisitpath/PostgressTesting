CREATE TABLE Students (
    StudentId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    DateOfBirth DATE,
    EnrollmentDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


SELECT * FROM Students;


INSERT INTO Students (FirstName, LastName, Email, DateOfBirth)
VALUES ('Jane', 'Smith', 'jane.smith@example.com', '1999-08-22');

-----------------------------------
CREATE OR REPLACE FUNCTION sp_getstudentbyid(p_id INT)
RETURNS TABLE(
    studentid INT,
    firstname VARCHAR(50),
    lastname VARCHAR(50),
    email VARCHAR(100),
    dateofbirth DATE,
    enrollmentdate TIMESTAMP
)
AS $$
BEGIN
    RETURN QUERY
    SELECT s.studentid, s.firstname, s.lastname, s.email, s.dateofbirth, s.enrollmentdate
    FROM students s
    WHERE s.studentid = p_id;
END;
$$ LANGUAGE plpgsql


-------------------------

SELECT * FROM sp_getstudentbyid(1);


----------------------

CREATE OR REPLACE PROCEDURE sp_updatestudent(
    p_id INT,
    p_firstname TEXT,
    p_lastname TEXT,
    p_email TEXT,
    p_dateofbirth TIMESTAMP,     -- ✅ use TIMESTAMP
    p_enrollmentdate TIMESTAMP   -- ✅ use TIMESTAMP
)
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE students
    SET firstname = p_firstname,
        lastname = p_lastname,
        email = p_email,
        dateofbirth = p_dateofbirth,
        enrollmentdate = p_enrollmentdate
    WHERE studentid = p_id;
END;
$$;



