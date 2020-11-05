SET IDENTITY_INSERT books ON;
INSERT INTO books(id, isbn, title, authors)
VALUES (1, '978-0-12-383872-8', 'Computer Architecture: A Quantitative Approach', 'Hennessy, Patterson'),
       (2, '978-0-12-385060-7', 'Computer Networks: A Systems Approach', 'Peterson, Davie'),
       (3, '978-0-13-300214-0', 'Compilers: Principles, Techniques, and Tools', 'Aho, Lam, Ullman, Sethi'),
       (4, '978-0-262-03293-3', 'Introduction To Algorithms', 'Cormen, Leiserson, Rivest, Stein');
SET IDENTITY_INSERT books OFF;

SET IDENTITY_INSERT bookcopies ON;
INSERT INTO bookcopies(copyId, bookId)
VALUES (1, 1),
       (2, 1),
       (3, 1),
       (4, 2),
       (5, 4),
       (6, 4),
       (7, 3);
SET IDENTITY_INSERT bookcopies OFF;

INSERT INTO loans(copyId, userId, due)
VALUES (1, '175639ec-6095-4cca-ae3e-1b8de96cf1b7', '2020-07-15'),
       (5, '175639ec-6095-4cca-ae3e-1b8de96cf1b7', '2020-11-07'),
       (2, 'd935cbfe-4420-484a-90c5-49d1ebeb6735', '2021-04-01'),
       (6, 'd35927b8-821a-4c90-9d8b-167856a40259', '2020-10-25');
