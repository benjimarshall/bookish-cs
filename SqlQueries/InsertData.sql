INSERT INTO users(username) VALUES (N'Benji');
INSERT INTO users(username) VALUES (N'Frank');
INSERT INTO users(username) VALUES (N'Frida');
INSERT INTO users(username) VALUES (N'Shiv');

INSERT INTO books(isbn, title, authors) VALUES (N'978-0-12-383872-8', N'Computer Architecture: A Quantitative Approach', N'Hennessy, Patterson');
INSERT INTO books(isbn, title, authors) VALUES (N'978-0-12-385060-7', N'Computer Networks: A Systems Approach', N'Peterson, Davie');
INSERT INTO books(isbn, title, authors) VALUES (N'978-0-13-300214-0', N'Compilers: Principles, Techniques, and Tools', N'Aho, Lam, Ullman, Sethi');
INSERT INTO books(isbn, title, authors) VALUES (N'978-0-262-03293-3', N'Introduction To Algorithms', N'Cormen, Leiserson, Rivest, Stein');

INSERT INTO bookcopies(isbn) VALUES (N'978-0-12-383872-8');
INSERT INTO bookcopies(isbn) VALUES (N'978-0-12-383872-8');
INSERT INTO bookcopies(isbn) VALUES (N'978-0-12-383872-8');
INSERT INTO bookcopies(isbn) VALUES (N'978-0-12-385060-7');
INSERT INTO bookcopies(isbn) VALUES (N'978-0-262-03293-3');
INSERT INTO bookcopies(isbn) VALUES (N'978-0-262-03293-3');

INSERT INTO loans(bookid, userid, due) VALUES (1, 1, '2020-07-15');
INSERT INTO loans(bookid, userid, due) VALUES (5, 1, '2020-11-07');
INSERT INTO loans(bookid, userid, due) VALUES (2, 2, '2021-04-01');
INSERT INTO loans(bookid, userid, due) VALUES (6, 4, '2020-10-25');

