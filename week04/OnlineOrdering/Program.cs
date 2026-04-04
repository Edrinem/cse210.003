// ── Order 1: US customer ───────────────────────────────────────
var address1  = new Address("742 Evergreen Terrace", "Springfield", "IL", "USA");
var customer1 = new Customer("Alice Johnson", address1);
var order1    = new Order(customer1);

order1.AddProduct(new Product("Wireless Mouse",    "WM-1042", 29.99, 1));
order1.AddProduct(new Product("USB-C Hub",         "UC-8831", 45.00, 2));
order1.AddProduct(new Product("Laptop Stand",      "LS-3301", 22.50, 1));

// ── Order 2: International customer ───────────────────────────
var address2  = new Address("12 Baker Street", "London", "England", "UK");
var customer2 = new Customer("James Hartley", address2);
var order2    = new Order(customer2);

order2.AddProduct(new Product("Mechanical Keyboard", "KB-5512", 89.99, 1));
order2.AddProduct(new Product("Monitor Light Bar",   "ML-2290", 34.00, 1));

// ── Display Order 1 ────────────────────────────────────────────
Console.WriteLine("==========================================");
Console.WriteLine(order1.GetPackingLabel());
Console.WriteLine(order1.GetShippingLabel());
Console.WriteLine($"Total Price: ${order1.GetTotalCost():F2}  (includes $5 US shipping)");

// ── Display Order 2 ────────────────────────────────────────────
Console.WriteLine("\n==========================================");
Console.WriteLine(order2.GetPackingLabel());
Console.WriteLine(order2.GetShippingLabel());
Console.WriteLine($"Total Price: ${order2.GetTotalCost():F2}  (includes $35 international shipping)");
