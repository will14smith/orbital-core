INSERT INTO badge ("Name", "Description", "Algorithm", "Category", "Multiple", "ImageUrl") VALUES
('60 on target', 'Get 60 arrows on the target in a 60 arrow round', 'hits:60:60', 'Skill', true, NULL),
('3 in the X', 'Get 3 consequtive arrows in the X scoring zone on an age suitable target face/distance combo', NULL, 'Skill', false, NULL);

INSERT INTO badge_holder ("BadgeId", "PersonId", "AwardedOn") VALUES
(1, 1, '2017-01-01'),
(1, 1, '2017-01-02'),
(2, 1, '2017-01-03'),
(1, 2, '2017-01-04');