SET IDENTITY_INSERT users ON;
INSERT INTO users(id, username)
VALUES (1, 'Benji.Marshall@softwire.com'),
       (2, 'Frank.Ma@softwire.com'),
       (3, 'Frida.Tviet@softwire.com'),
       (4, 'Shivam.Chandarana@softwire.com');
SET IDENTITY_INSERT users OFF;

INSERT INTO books(isbn, title, authors)
VALUES ('978-0-12-383872-8', 'Computer Architecture: A Quantitative Approach', 'Hennessy, Patterson'),
       ('978-0-12-385060-7', 'Computer Networks: A Systems Approach', 'Peterson, Davie'),
       ('978-0-13-300214-0', 'Compilers: Principles, Techniques, and Tools', 'Aho, Lam, Ullman, Sethi'),
       ('978-0-262-03293-3', 'Introduction To Algorithms', 'Cormen, Leiserson, Rivest, Stein');

SET IDENTITY_INSERT bookcopies ON;
INSERT INTO bookcopies(id, isbn)
VALUES (1, '978-0-12-383872-8'),
       (2, '978-0-12-383872-8'),
       (3, '978-0-12-383872-8'),
       (4, '978-0-12-385060-7'),
       (5, '978-0-262-03293-3'),
       (6, '978-0-262-03293-3');
       (7, '978-0-13-300214-0');
SET IDENTITY_INSERT bookcopies OFF;

INSERT INTO loans(bookid, userid, due)
VALUES (1, 1, '2020-07-15'),
       (5, 1, '2020-11-07'),
       (2, 2, '2021-04-01'),
       (6, 4, '2020-10-25');

