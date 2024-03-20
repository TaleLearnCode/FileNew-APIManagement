SET IDENTITY_INSERT dbo.Quote ON
GO

MERGE INTO dbo.Quote AS TARGET
USING (
    VALUES
       ( 1, 'Steve Jobs',            'The only way to do great work is to love what you do.'),
       ( 2, 'Sun Tzu',               'In the midst of chaos, there is also opportunity.'),
       ( 3, 'Alan Kay',              'The best way to predict the future is to invent it.'),
       ( 4, 'Winston Churchill',     'Success is not final, failure is not fatal: It is the courage to continue that counts.'),
       ( 5, 'Franklin D. Roosevelt', 'The only thing we have to fear is fear itself.'),
       ( 6, 'Allen Sauders',         'Life is what happens when you''re busy making other plans.'),
       ( 7, 'Wayne Gretzky',         'You miss 100% of the shots you don''t take.'),
       ( 8, 'Nelson Mandela',        'The greatest glory in living lies not in never falling, but in rising every time we fall.'),
       ( 9, 'Franklin D. Roosevelt', 'The only limit to our realization of tomorrow will be our doubts of today.'),
       (10, 'Confucius',             'It does not matter how slowly you go as long as you do not stop.'),
       (11, 'Lao Tzu',               'A journey of a thousand miles begins with a single step.'),
       (12, 'Nelson Mandela',        'It always seems impossible until it''s done.'),
       (13, 'Socrates',              'The only true wisdom is in knowing you know nothing.'),
       (14, 'Theodore Roosevelt',    'Believe you can and you''re halfway there.'),
       (15, 'Ronald Reagan',         'We can''t help everyone, but everyone can help someone.'),
       (16, 'Mark Twain',            'The secret of getting ahead is getting started.'),
       (17, 'Mahatma Gandhi',        'You must be the change you wish to see in the world.'),
       (18, 'Ralph Waldo Emerson',   'The only way to have a friend is to be one.'),
       (19, 'Dalai Lama',            'Happiness is not something ready made. It comes from your own actions.'),
       (20, 'Ralph Waldo Emerson',   'To be yourself in a world that is constantly trying to make you something else is the greatest accomplishment.'),
       (21, 'Anonymous',             'The only person you should try to be better than is the person you were yesterday.'),
       (22, 'Plato',                 'The greatest wealth is to live content with little.'),
       (23, 'Robert Frost',          'In three words I can sum up everything I''ve learned about life: It goes on.'),
       (24, 'Heraclitus',            'The only constant in life is change.'),
       (25, 'Ralph Waldo Emerson',   'Do not go where the path may lead, go instead where there is no path and leave a trail.'),
       (26, 'Albert Schweitzer',     'Success is not the key to happiness. Happiness is the key to success. If you love what you are doing, you will be successful.'),
       (27, 'Marie Curie',           'Nothing in life is to be feared, it is only to be understood. Now is the time to understand more, so that we may fear less.'),
       (28, 'Zig Ziglar',            'You don''t have to be great to start, but you have to start to be great.'),
       (29, 'Arthur C. Clark',       'The only way of finding the limits of the possible is by going beyond them into the impossible.'),
       (30, 'Eleanor Roosevelt',     'The future belongs to those who believe in the beauty of their dreams.'),
       (31, 'Winston Churchill',     'Success consists of going from failure to failure without loss of enthusiasm.'),
       (32, 'George Eliot',          'It is never too late to be what you might have been.'),
       (33, 'Albert Einstein',       'Strive not to be a success, but rather to be of value.'),
       (34, 'William Butler Yeats',  'Do not wait to strike till the iron is hot; but make it hot by striking.'),
       (35, 'Robert Frost',          'The best way out is always through.'),
       (36, 'Booker T. Washington',  'If you want to lift yourself up, lift up someone else.'),
       (37, 'Charles R. Swindoll',   'Life is 10% what happens to us and 90% how we react to it.'),
       (38, 'Lou Holtz',             'It''s not the load that breaks you down, it''s the way you carry it.'),
       (39, 'Stephen Covey',         'I am not a product of my circumstances. I am a product of my decisions.'),
       (40, 'Babe Ruth',             'Don''t let the fear of striking out hold you back.'))
AS SOURCE (QuoteId, Author, Content)
ON TARGET.QuoteId = SOURCE.QuoteId
WHEN MATCHED THEN UPDATE SET TARGET.Author = SOURCE.Author,
                             TARGET.Content  = SOURCE.Content
WHEN NOT MATCHED BY TARGET THEN INSERT (QuoteId,
                                        Author,
                                        Content)
                                 VALUES (SOURCE.QuoteId,
                                         SOURCE.Author,
                                         SOURCE.Content)
WHEN NOT MATCHED BY SOURCE THEN DELETE;
GO

SET IDENTITY_INSERT dbo.Quote OFF
GO