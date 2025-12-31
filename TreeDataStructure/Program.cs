using TreeDataStructure;

// Demonstrate the new comprehensive enterprise tree data structure implementation
Console.WriteLine("=== Enterprise Tree Data Structure Implementation ===\n");

// 1. Binary Search Tree with integer values and advanced operations
Console.WriteLine("1. Binary Search Tree (Generic) with integers and advanced operations:");
var intBst = new BinarySearchTree<int>();

int[] values = { 50, 30, 70, 20, 40, 60, 80, 10, 25, 35, 45 };
intBst.InsertRange(values);

Console.WriteLine($"Tree height: {intBst.Height}");
Console.WriteLine($"Tree count: {intBst.Count}");
Console.WriteLine($"Min value: {intBst.GetMin()}");
Console.WriteLine($"Max value: {intBst.GetMax()}");
Console.WriteLine($"Is empty: {intBst.IsEmpty}");
Console.WriteLine($"Is balanced: {intBst.IsBalanced()}");
Console.WriteLine($"Is valid BST: {intBst.IsValidBST()}");

Console.WriteLine("\nIn-Order Traversal: " + string.Join(" ", intBst.InOrderTraversal()));
Console.WriteLine("Pre-Order Traversal: " + string.Join(" ", intBst.PreOrderTraversal()));
Console.WriteLine("Post-Order Traversal: " + string.Join(" ", intBst.PostOrderTraversal()));
Console.WriteLine("Level-Order Traversal: " + string.Join(" ", intBst.LevelOrderTraversal()));
Console.WriteLine("Reverse In-Order Traversal: " + string.Join(" ", intBst.ReverseInOrderTraversal()));

// Advanced operations
Console.WriteLine($"\nDepth of 40: {intBst.GetDepth(40)}");
Console.WriteLine($"Values at level 2: {string.Join(", ", intBst.GetValuesAtLevel(2))}");
Console.WriteLine($"Path to value 35: {string.Join(" -> ", intBst.GetPathToValue(35))}");
Console.WriteLine($"Kth smallest (3rd): {intBst.GetKthSmallest(3)}");
Console.WriteLine($"Kth largest (3rd): {intBst.GetKthLargest(3)}");

// Test find and contains
Console.WriteLine($"\nContains 40: {intBst.Contains(40)}");
Console.WriteLine($"Contains 100: {intBst.Contains(100)}");
var node = intBst.Find(40);
Console.WriteLine($"Node with value 40 found: {node != null} (Parent: {(node?.Parent?.Data ?? default(int))})");

// Test removal
Console.WriteLine("\nAfter removing 30:");
intBst.Remove(30);
Console.WriteLine("In-Order Traversal: " + string.Join(" ", intBst.InOrderTraversal()));
Console.WriteLine($"Tree count after removal: {intBst.Count}");

// 2. String values
Console.WriteLine("\n2. Binary Search Tree with strings:");
var stringBst = new BinarySearchTree<string>(new[] { "banana", "apple", "orange", "grape", "kiwi" });
Console.WriteLine("In-Order Traversal: " + string.Join(" ", stringBst.InOrderTraversal()));
Console.WriteLine($"Min value: {stringBst.GetMin()}");
Console.WriteLine($"Max value: {stringBst.GetMax()}");

// 3. AVL Tree for balanced operations
Console.WriteLine("\n3. AVL Tree (Self-Balancing):\n");
var avlTree = new AvlTree<int>();

// Insert values in a way that would cause imbalance in a regular BST
int[] avlValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
foreach (int value in avlValues)
{
    avlTree.Insert(value);
}

Console.WriteLine($"AVL Tree height: {avlTree.Height} (much better than regular BST which would be {avlValues.Length})");
Console.WriteLine($"AVL Tree count: {avlTree.Count}");
Console.WriteLine($"Is balanced: {avlTree.IsBalanced()}");
Console.WriteLine("In-Order Traversal: " + string.Join(" ", avlTree.InOrderTraversal()));

// 4. Red-Black Tree implementation
Console.WriteLine("\n4. Red-Black Tree (Self-Balancing with different approach):\n");
var rbTree = new RedBlackTree<int>();
int[] rbValues = { 10, 20, 30, 15, 25, 5, 1 };
foreach (int value in rbValues)
{
    rbTree.Insert(value);
}

Console.WriteLine($"Red-Black Tree height: {rbTree.Height}");
Console.WriteLine($"Red-Black Tree count: {rbTree.Count}");
Console.WriteLine($"Is balanced: {rbTree.IsBalanced()}");
Console.WriteLine("In-Order Traversal: " + string.Join(" ", rbTree.InOrderTraversal()));
Console.WriteLine("Serialized: " + rbTree.Serialize());

// 5. B-Tree implementation for disk-based operations
Console.WriteLine("\n5. B-Tree (Optimized for disk-based operations):\n");
var bTree = new BTree<int>(3); // Minimum degree of 3
int[] bTreeValues = { 10, 20, 5, 6, 12, 30, 7, 17, 15, 18, 22, 25, 40, 50, 2, 16, 27, 53 };
foreach (int value in bTreeValues)
{
    bTree.Insert(value);
}

Console.WriteLine($"B-Tree height: {bTree.Height}");
Console.WriteLine($"B-Tree count: {bTree.Count}");
Console.WriteLine($"Is balanced: {bTree.IsBalanced()}");
Console.WriteLine("In-Order Traversal: " + string.Join(" ", bTree.InOrderTraversal()));

// 6. Serialization and Deserialization
Console.WriteLine("\n6. Tree Serialization:");
var originalTree = new BinarySearchTree<int>(new[] { 5, 3, 8, 2, 4, 7, 9 });
string serialized = originalTree.Serialize();
Console.WriteLine($"Serialized tree: {serialized}");

var deserializedTree = new BinarySearchTree<int>();
deserializedTree.Deserialize(serialized);
Console.WriteLine($"Deserialized tree: {string.Join(" ", deserializedTree.InOrderTraversal())}");

// 7. Cloning functionality
Console.WriteLine("\n7. Tree Cloning:");
var clonedTree = (BinarySearchTree<int>)originalTree.Clone();
Console.WriteLine($"Original tree: {string.Join(" ", originalTree.InOrderTraversal())}");
Console.WriteLine($"Cloned tree: {string.Join(" ", clonedTree.InOrderTraversal())}");

// 8. Advanced search operations
Console.WriteLine("\n8. Advanced Search Operations:");
var searchTree = new BinarySearchTree<int>(new[] { 50, 30, 70, 20, 40, 60, 80 });
var bfsResult = searchTree.BreadthFirstSearch(40);
var dfsResult = searchTree.DepthFirstSearch(40);
Console.WriteLine($"BFS for 40: {(bfsResult != null ? bfsResult.Data : "Not found")}");
Console.WriteLine($"DFS for 40: {(dfsResult != null ? dfsResult.Data : "Not found")}");

var lcaResult = searchTree.GetLowestCommonAncestor(20, 40);
Console.WriteLine($"LCA of 20 and 40: {(lcaResult != null ? lcaResult.Data : "Not found")}");

// 9. Tree Factory Pattern
Console.WriteLine("\n9. Tree Factory Pattern:");
var factoryBst = TreeFactory.CreateTree<int>(TreeType.BinarySearchTree);
factoryBst.InsertRange(new[] { 50, 30, 70, 20, 40 });
Console.WriteLine($"Factory BST: {string.Join(" ", factoryBst.InOrderTraversal())}");

var factoryAvl = TreeFactory.CreateTree<int>(TreeType.AvlTree, new[] { 1, 2, 3, 4, 5 });
Console.WriteLine($"Factory AVL: {string.Join(" ", factoryAvl.InOrderTraversal())}");

var factoryRb = TreeFactory.CreateTree<int>(TreeType.RedBlackTree, new[] { 10, 20, 30 });
Console.WriteLine($"Factory Red-Black: {string.Join(" ", factoryRb.InOrderTraversal())}");

var factoryB = TreeFactory.CreateTree<int>(TreeType.BTree, new[] { 10, 20, 5, 6, 12 }, 3);
Console.WriteLine($"Factory B-Tree: {string.Join(" ", factoryB.InOrderTraversal())}");

// 10. Performance comparison example (conceptual)
Console.WriteLine("\n10. Real-world enterprise capabilities:");
Console.WriteLine("- Generic implementation supporting any IComparable type");
Console.WriteLine("- Multiple tree types: BST, AVL, Red-Black, B-Tree");
Console.WriteLine("- Self-balancing trees for guaranteed O(log n) operations");
Console.WriteLine("- Comprehensive traversal algorithms (in-order, pre-order, post-order, level-order, reverse in-order)");
Console.WriteLine("- Serialization/deserialization for persistence");
Console.WriteLine("- Proper error handling and validation");
Console.WriteLine("- Parent references for bidirectional navigation");
Console.WriteLine("- IEnumerable interface for LINQ compatibility");
Console.WriteLine("- Thread-safe operation patterns (implementation-ready)");
Console.WriteLine("- Memory-efficient node structure");
Console.WriteLine("- Advanced operations: depth, path finding, LCA, kth elements");
Console.WriteLine("- Range operations and bulk insertions");
Console.WriteLine("- Multiple balancing strategies for different use cases");
Console.WriteLine("- Factory pattern for easy tree creation and switching");
Console.WriteLine("- Abstract base class for common functionality and extensibility");

Console.WriteLine("\n=== Enterprise Tree Implementation Complete ===");
