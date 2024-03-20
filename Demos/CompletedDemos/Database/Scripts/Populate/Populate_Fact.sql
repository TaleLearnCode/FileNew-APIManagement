SET IDENTITY_INSERT dbo.Fact ON
GO

MERGE INTO dbo.Fact AS TARGET
USING (VALUES ( 1, 'The Earth''s core is hotter than the surface of the Sun.'),
              ( 2, 'The shortest war in history lasted only 38 minutes. It was between Britain and Zanzibar on August 27, 1896.'),
              ( 3, 'Octopuses have three hearts.'),
              ( 4, 'The Great Wall of China is not visible from space with the naked eye.'),
              ( 5, 'Honey never spoils. Archaeologists have found pots of honey in ancient Egyptian tombs that are over 3,000 years old and still perfectly edible.'),
              ( 6, 'The average person will spend six months of their life waiting for red lights to turn green.'),
              ( 7, 'Cleopatra VII of Egypt lived closer in time to the first Moon landing than to the construction of the Great Pyramid of Giza.'),
              ( 8, 'The largest volcano in the solar system is Olympus Mons on Mars. It''s about 13.6 miles (22 kilometers) high.'),
              ( 9, 'There are more possible iterations of a game of chess than there are atoms in the known universe.'),
              (10, 'The electric chair was invented by a dentist.'),
              (11, 'Peanuts are not nuts; they are legumes.'),
              (12, 'The human brain is more active during sleep than during the day when awake.'),
              (13, 'The unicorn is the national animal of Scotland.'),
              (14, 'The smell of freshly cut grass is actually a plant distress call.'),
              (15, 'A group of flamingos is called a flamboyance.'),
              (16, 'The microwave was invented after a researcher walked by a radar tube and a chocolate bar melted in his pocket.'),
              (17, 'The human eye can distinguish about 10 million different colors.'),
              (18, 'Cows have best friends and get stressed when they are separated.'),
              (19, 'The total weight of all the ants on Earth is approximately equal to the total weight of all the humans on Earth.'),
              (20, 'The Eiffel Tower can be 15 cm taller during the summer due to thermal expansion.'),
              (21, 'Bananas are berries, but strawberries are not.'),
              (22, 'Armadillos can be carriers of leprosy.'),
              (23, 'The longest time between two twins being born is 87 days.'),
              (24, 'Coca-Cola was originally green.'),
              (25, 'The moon is moving away from the Earth at a rate of about 1.5 inches (3.8 cm) per year.'),
              (26, 'The fingerprints of koala bears are virtually indistinguishable from those of humans, so much so that they could be confused at a crime scene.'),
              (27, 'A crocodile cannot stick its tongue out.'),
              (28, 'Banging your head against a wall burns 150 calories an hour.'),
              (29, 'A day on Venus is longer than a year on Venus. It takes Venus 243 Earth days to complete one rotation on its axis and 225 Earth days to orbit the Sun.'),
              (30, 'The first oranges were not orange; they were green.'),
              (31, '''The quick brown fox jumps over the lazy dog'' uses every letter of the alphabet.'),
              (32, 'The longest English word without a vowel is ''rhythms.'''),
              (33, 'There are more tigers held privately as pets than there are in the wild.'),
              (34, 'Penguins can jump up to 6 feet (1.8 meters) out of the water.'),
              (35, 'Some cats are allergic to humans.'),
              (36, 'A snail can sleep for three years.'),
              (37, 'The world''s largest desert is not the Sahara; it''s Antarctica.')) 
AS SOURCE (FactId, Content)
ON TARGET.FactId = SOURCE.FactId
WHEN MATCHED THEN UPDATE SET TARGET.Content = SOURCE.Content
WHEN NOT MATCHED BY TARGET THEN INSERT (FactId,
                                        Content)
                                 VALUES (SOURCE.FactId,
                                         SOURCE.Content)
WHEN NOT MATCHED BY SOURCE THEN DELETE;
GO

SET IDENTITY_INSERT dbo.Fact OFF
GO