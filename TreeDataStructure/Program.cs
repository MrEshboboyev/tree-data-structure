using TreeDataStructure;

BinarySearchTree bst = new();

bst.Insert(50);
bst.Insert(30);
bst.Insert(70);
bst.Insert(20);
bst.Insert(40);
bst.Insert(60);
bst.Insert(80);

Console.WriteLine("Daraxt elementlari (In-Order Traversal):");

BinarySearchTree.InOrderTraversal(bst.Root);

// Output: Daraxt elementlari (In-Order Traversal): 20 30 40 50 60 70 80 
