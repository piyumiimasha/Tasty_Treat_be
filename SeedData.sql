-- Seed Data for TastyTreatDb
-- Run this script after creating the database with migrations

USE TastyTreatDb;
GO

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

-- ====================================
-- CLEAR ALL EXISTING DATA
-- ====================================
-- Disable all foreign key constraints
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
GO

-- Delete all data in reverse order
DELETE FROM [Payment];
DELETE FROM [Instant_Quote];
DELETE FROM [Delivery];
DELETE FROM [Review];
DELETE FROM [Order_Item];
DELETE FROM [Chat_msg];
DELETE FROM [Order];
DELETE FROM [Customization_option];
DELETE FROM [Item];
DELETE FROM [User];
GO

-- Re-enable all foreign key constraints
EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL';
GO

PRINT 'All existing data cleared successfully!';
GO

-- ====================================
-- INSERT NEW DATA
-- ====================================

-- Insert Users (5 rows)
SET IDENTITY_INSERT [User] ON;
INSERT INTO [User] (User_Id, Name, Email, Role, Password, PhoneNo, Address, created_at, updated_at)
VALUES 
(1, 'John Smith', 'john.smith@email.com', 'Customer', '$2a$11$hashedpassword1', '555-0101', '123 Main St, New York, NY', GETUTCDATE(), GETUTCDATE()),
(2, 'Sarah Johnson', 'sarah.j@email.com', 'Customer', '$2a$11$hashedpassword2', '555-0102', '456 Oak Ave, Los Angeles, CA', GETUTCDATE(), GETUTCDATE()),
(3, 'Mike Wilson', 'mike.w@email.com', 'DeliveryPerson', '$2a$11$hashedpassword3', '555-0103', '789 Pine Rd, Chicago, IL', GETUTCDATE(), GETUTCDATE()),
(4, 'Emily Davis', 'emily.d@email.com', 'Customer', '$2a$11$hashedpassword4', '555-0104', '321 Elm St, Houston, TX', GETUTCDATE(), GETUTCDATE()),
(5, 'David Brown', 'david.b@email.com', 'DeliveryPerson', '$2a$11$hashedpassword5', '555-0105', '654 Maple Dr, Phoenix, AZ', GETUTCDATE(), GETUTCDATE());
SET IDENTITY_INSERT [User] OFF;
GO

-- Insert Items (5 rows)
SET IDENTITY_INSERT [Item] ON;
INSERT INTO [Item] (item_id, Name, Category, base_price, base_price_unit, Description)
VALUES 
(1, 'Classic Chocolate Cake', 'Layer Cakes', 45.99, 'per cake', 'Three-layer chocolate cake with rich chocolate ganache and buttercream frosting'),
(2, 'Red Velvet Cake', 'Layer Cakes', 49.99, 'per cake', 'Moist red velvet layers with cream cheese frosting and white chocolate shavings'),
(3, 'Vanilla Cupcakes', 'Cupcakes', 3.50, 'per cupcake', 'Light and fluffy vanilla cupcakes topped with buttercream swirl'),
(4, 'Strawberry Shortcake', 'Specialty Cakes', 42.99, 'per cake', 'Fresh strawberries layered with vanilla sponge and whipped cream'),
(5, 'Chocolate Chip Cookies', 'Cookies', 2.99, 'per cookie', 'Classic homemade chocolate chip cookies baked fresh daily');
SET IDENTITY_INSERT [Item] OFF;
GO

-- Insert Customization Options (5 rows)
SET IDENTITY_INSERT [Customization_option] ON;
INSERT INTO [Customization_option] (option_id, item_id, Name, Type, additional_price, updated_at)
VALUES 
(1, 1, 'Add Custom Message', 'Decoration', 5.00, GETUTCDATE()),
(2, 1, 'Extra Layer', 'Size', 12.00, GETUTCDATE()),
(3, 2, 'Gold Leaf Decoration', 'Decoration', 15.00, GETUTCDATE()),
(4, 3, 'Buttercream Flavor (Vanilla/Chocolate/Strawberry)', 'Frosting', 0.50, GETUTCDATE()),
(5, 4, 'Add Fresh Berries', 'Topping', 8.00, GETUTCDATE());
SET IDENTITY_INSERT [Customization_option] OFF;
GO

-- Insert Orders (5 rows)
SET IDENTITY_INSERT [Order] ON;
INSERT INTO [Order] (order_id, customer_id, Status, delivery_address, special_instructions, total_amount, order_date)
VALUES 
(1, 1, 'Delivered', '123 Main St, New York, NY', 'Ring doorbell twice', 28.48, DATEADD(day, -5, GETUTCDATE())),
(2, 2, 'In Progress', '456 Oak Ave, Los Angeles, CA', 'Leave at door', 35.97, DATEADD(day, -2, GETUTCDATE())),
(3, 4, 'Pending', '321 Elm St, Houston, TX', 'Call on arrival', 19.98, DATEADD(hour, -3, GETUTCDATE())),
(4, 1, 'Cancelled', '123 Main St, New York, NY', NULL, 45.96, DATEADD(day, -10, GETUTCDATE())),
(5, 2, 'Ready for Pickup', '456 Oak Ave, Los Angeles, CA', 'Extra napkins please', 21.98, DATEADD(hour, -1, GETUTCDATE()));
SET IDENTITY_INSERT [Order] OFF;
GO

-- Insert Chat Messages (5 rows)
SET IDENTITY_INSERT [Chat_msg] ON;
INSERT INTO [Chat_msg] (msg_id, sender_id, msg_txt, is_read, created_at)
VALUES 
(1, 1, 'Hello, I have a question about my order #1', 0, DATEADD(hour, -2, GETUTCDATE())),
(2, 2, 'Can I change my delivery address?', 1, DATEADD(day, -1, GETUTCDATE())),
(3, 4, 'Is the kitchen still open?', 1, DATEADD(hour, -5, GETUTCDATE())),
(4, 1, 'Thank you for the quick delivery!', 1, DATEADD(day, -5, GETUTCDATE())),
(5, 2, 'What are your operating hours?', 0, DATEADD(minute, -30, GETUTCDATE()));
SET IDENTITY_INSERT [Chat_msg] OFF;
GO

-- Insert Order Items (5 rows)
SET IDENTITY_INSERT [Order_Item] ON;
INSERT INTO [Order_Item] (order_item_id, order_id, item_id, customization_option_id, quantity, order_item_price, is_available, created_at)
VALUES 
(1, 1, 1, 1, 1, 15.49, 1, DATEADD(day, -5, GETUTCDATE())),
(2, 1, 2, NULL, 1, 8.99, 1, DATEADD(day, -5, GETUTCDATE())),
(3, 2, 5, 5, 2, 35.98, 1, DATEADD(day, -2, GETUTCDATE())),
(4, 3, 3, 4, 1, 11.49, 1, DATEADD(hour, -3, GETUTCDATE())),
(5, 5, 4, NULL, 2, 13.98, 1, DATEADD(hour, -1, GETUTCDATE()));
SET IDENTITY_INSERT [Order_Item] OFF;
GO

-- Insert Reviews (5 rows)
SET IDENTITY_INSERT [Review] ON;
INSERT INTO [Review] (review_id, order_id, item_id, customer_id, Comment, Rating, review_date)
VALUES 
(1, 1, 1, 1, 'Absolutely stunning chocolate cake! Moist and delicious, everyone loved it!', 5, DATEADD(day, -4, GETUTCDATE())),
(2, 1, 2, 1, 'Beautiful red velvet cake, the cream cheese frosting was perfect.', 5, DATEADD(day, -4, GETUTCDATE())),
(3, 2, 5, 2, 'Best cookies I have ever tasted! Will definitely order again.', 5, DATEADD(day, -1, GETUTCDATE())),
(4, 3, 3, 4, 'Cupcakes were good but a bit dry. Frosting was excellent though.', 3, DATEADD(hour, -2, GETUTCDATE())),
(5, 5, 4, 2, 'The strawberry shortcake was heavenly! Fresh berries and light cream.', 5, DATEADD(hour, -30, GETUTCDATE()));
SET IDENTITY_INSERT [Review] OFF;
GO

-- Insert Deliveries (5 rows)
SET IDENTITY_INSERT [Delivery] ON;
INSERT INTO [Delivery] (delivery_id, order_id, delivery_person_id, delivery_status, delivery_notes, assigned_at, picked_up_at, delivered_at)
VALUES 
(1, 1, 3, 'Delivered', 'Customer was very pleased', DATEADD(day, -5, GETUTCDATE()), DATEADD(day, -5, DATEADD(minute, 15, GETUTCDATE())), DATEADD(day, -5, DATEADD(minute, 45, GETUTCDATE()))),
(2, 2, 5, 'In Transit', 'On the way to customer', DATEADD(day, -2, GETUTCDATE()), DATEADD(day, -2, DATEADD(minute, 10, GETUTCDATE())), NULL),
(3, 3, 3, 'Assigned', 'Waiting for kitchen to finish', DATEADD(hour, -3, GETUTCDATE()), NULL, NULL),
(4, 4, 5, 'Cancelled', 'Customer cancelled order', DATEADD(day, -10, GETUTCDATE()), NULL, NULL),
(5, 5, 3, 'Ready for Pickup', 'Order is ready', DATEADD(hour, -1, GETUTCDATE()), DATEADD(minute, -50, GETUTCDATE()), NULL);
SET IDENTITY_INSERT [Delivery] OFF;
GO

-- Insert Instant Quotes (5 rows)
SET IDENTITY_INSERT [Instant_Quote] ON;
INSERT INTO [Instant_Quote] (quote_id, customer_id, Converted_order_id, Items, Tax, Discount, delivery_fee, estimated_price, created_at)
VALUES 
(1, 1, 1, '[{"itemId":1,"quantity":1},{"itemId":2,"quantity":1}]', 2.00, 0.00, 4.00, 28.48, DATEADD(day, -5, DATEADD(minute, -10, GETUTCDATE()))),
(2, 2, 2, '[{"itemId":5,"quantity":2}]', 2.99, 5.00, 3.00, 35.97, DATEADD(day, -2, DATEADD(minute, -5, GETUTCDATE()))),
(3, 4, NULL, '[{"itemId":3,"quantity":1},{"itemId":4,"quantity":1}]', 1.80, 0.00, 3.50, 23.28, DATEADD(hour, -6, GETUTCDATE())),
(4, 1, NULL, '[{"itemId":1,"quantity":2}]', 2.60, 2.00, 4.00, 30.58, DATEADD(minute, -45, GETUTCDATE())),
(5, 2, 5, '[{"itemId":4,"quantity":2}]', 1.40, 0.00, 4.00, 19.38, DATEADD(hour, -1, DATEADD(minute, -2, GETUTCDATE())));
SET IDENTITY_INSERT [Instant_Quote] OFF;
GO

-- Insert Payments (5 rows)
SET IDENTITY_INSERT [Payment] ON;
INSERT INTO [Payment] (payment_id, order_id, transaction_id, payment_method, Amount, payment_status, transaction_date)
VALUES 
(1, 1, 'TXN-2026011501', 'Credit Card', 28.48, 'Completed', DATEADD(day, -5, GETUTCDATE())),
(2, 2, 'TXN-2026011802', 'PayPal', 35.97, 'Completed', DATEADD(day, -2, GETUTCDATE())),
(3, 3, 'TXN-2026012003', 'Credit Card', 19.98, 'Pending', DATEADD(hour, -3, GETUTCDATE())),
(4, 4, 'TXN-2026011004', 'Cash', 45.96, 'Refunded', DATEADD(day, -10, GETUTCDATE())),
(5, 5, 'TXN-2026012005', 'Debit Card', 21.98, 'Completed', DATEADD(hour, -1, GETUTCDATE()));
SET IDENTITY_INSERT [Payment] OFF;
GO

-- Verify data was inserted
PRINT '===== DATA VERIFICATION =====';
PRINT 'Users: ' + CAST((SELECT COUNT(*) FROM [User]) AS VARCHAR);
PRINT 'Items: ' + CAST((SELECT COUNT(*) FROM [Item]) AS VARCHAR);
PRINT 'Customization Options: ' + CAST((SELECT COUNT(*) FROM [Customization_option]) AS VARCHAR);
PRINT 'Orders: ' + CAST((SELECT COUNT(*) FROM [Order]) AS VARCHAR);
PRINT 'Chat Messages: ' + CAST((SELECT COUNT(*) FROM [Chat_msg]) AS VARCHAR);
PRINT 'Order Items: ' + CAST((SELECT COUNT(*) FROM [Order_Item]) AS VARCHAR);
PRINT 'Reviews: ' + CAST((SELECT COUNT(*) FROM [Review]) AS VARCHAR);
PRINT 'Deliveries: ' + CAST((SELECT COUNT(*) FROM [Delivery]) AS VARCHAR);
PRINT 'Instant Quotes: ' + CAST((SELECT COUNT(*) FROM [Instant_Quote]) AS VARCHAR);
PRINT 'Payments: ' + CAST((SELECT COUNT(*) FROM [Payment]) AS VARCHAR);
PRINT 'Seed data inserted successfully!';
GO
