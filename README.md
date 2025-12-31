# Enterprise Tree Data Structure Library

A comprehensive, generic, and enterprise-ready tree data structure library implemented in C# with support for multiple tree types, advanced operations, and extensive functionality.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Supported Tree Types](#supported-tree-types)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [API Reference](#api-reference)
- [Advanced Operations](#advanced-operations)
- [Performance Characteristics](#performance-characteristics)
- [Examples](#examples)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)

## Overview

This library provides a complete implementation of various tree data structures with a focus on performance, functionality, and extensibility. It includes multiple tree types optimized for different use cases, comprehensive traversal algorithms, and advanced tree operations.

## Features

- **Generic Implementation**: Support for any type that implements `IComparable<T>`
- **Multiple Tree Types**: Binary Search Tree, AVL Tree, Red-Black Tree, and B-Tree
- **Comprehensive Operations**: Insert, remove, search, traversal, and advanced operations
- **Self-Balancing Trees**: AVL and Red-Black trees for guaranteed O(log n) operations
- **Multiple Traversal Algorithms**: In-order, pre-order, post-order, level-order, and reverse in-order
- **Advanced Operations**: Lowest Common Ancestor (LCA), kth smallest/largest, range queries
- **Factory Pattern**: Easy creation of different tree types
- **IEnumerable Support**: LINQ compatibility
- **Parent References**: Bidirectional navigation support
- **Memory Efficient**: Optimized node structure
- **Thread-Safe Ready**: Implementation patterns for thread safety

## Supported Tree Types

### Binary Search Tree (BST)
- Basic binary search tree implementation
- O(log n) average case operations, O(n) worst case
- Supports all basic operations and traversals

### AVL Tree
- Self-balancing binary search tree
- Guaranteed O(log n) operations through rotation-based balancing
- Maintains strict height balance

### Red-Black Tree
- Self-balancing binary search tree with color-based balancing
- Guaranteed O(log n) operations
- Less rigid balancing than AVL, potentially faster insertions/deletions

### B-Tree
- Disk-optimized tree structure
- Multiple keys per node, efficient for large datasets
- Configurable degree for optimization

## Installation

1. Clone the repository:
```bash
git clone https://github.com/your-username/tree-data-structure.git
```

2. Open the solution in Visual Studio or build using .NET CLI:
```bash
dotnet build TreeDataStructure.sln
```

3. Run the application:
```bash
dotnet run --project TreeDataStructure/TreeDataStructure.csproj
```

## Quick Start

```csharp
using TreeDataStructure;

// Create a binary search tree for integers
var bst = new BinarySearchTree<int>();

// Insert values
bst.Insert(50);
bst.Insert(30);
bst.Insert(70);
bst.InsertRange(new[] { 20, 40, 60, 80 });

// Perform operations
Console.WriteLine($"Tree height: {bst.Height}");
Console.WriteLine($"Tree count: {bst.Count}");
Console.WriteLine($"Min value: {bst.GetMin()}");
Console.WriteLine($"Max value: {bst.GetMax()}");

// Traversals
Console.WriteLine("In-Order Traversal: " + string.Join(" ", bst.InOrderTraversal()));
Console.WriteLine("Level-Order Traversal: " + string.Join(" ", bst.LevelOrderTraversal()));

// Advanced operations
Console.WriteLine($"Is balanced: {bst.IsBalanced()}");
Console.WriteLine($"Is valid BST: {bst.IsValidBST()}");
```

## API Reference

### ITree<T> Interface

All tree implementations implement the `ITree<T>` interface with the following members:

#### Properties
- `Count`: Number of elements in the tree
- `Root`: Root node of the tree
- `IsEmpty`: Whether the tree is empty
- `Height`: Height of the tree

#### Methods
- `Insert(T value)`: Insert a value into the tree
- `InsertRange(IEnumerable<T> values)`: Insert multiple values
- `Remove(T value)`: Remove a value from the tree
- `Contains(T value)`: Check if a value exists
- `Find(T value)`: Find a node containing the value
- `Clear()`: Remove all elements
- `InOrderTraversal()`: Get values in sorted order
- `PreOrderTraversal()`: Get values in pre-order
- `PostOrderTraversal()`: Get values in post-order
- `LevelOrderTraversal()`: Get values in level order
- `ReverseInOrderTraversal()`: Get values in reverse sorted order
- `GetMin()`: Get minimum value
- `GetMax()`: Get maximum value
- `GetHeight(TreeNode<T> node)`: Get height of a node
- `GetDepth(TreeNode<T> node)`: Get depth of a node
- `GetPathToNode(T value)`: Get path from root to a node
- `GetNodesAtLevel(int level)`: Get all nodes at a specific level
- `BreadthFirstSearch(T value)`: BFS for a value
- `DepthFirstSearch(T value)`: DFS for a value
- `GetLowestCommonAncestor(T value1, T value2)`: Get LCA of two nodes
- `IsValidBST()`: Validate BST property
- `GetKthSmallest(int k)`: Get kth smallest element
- `GetKthLargest(int k)`: Get kth largest element

### Tree Factory

Use the `TreeFactory` to create different tree types:

```csharp
// Create different tree types
var bst = TreeFactory.CreateTree<int>(TreeType.BinarySearchTree);
var avl = TreeFactory.CreateTree<int>(TreeType.AvlTree);
var rb = TreeFactory.CreateTree<int>(TreeType.RedBlackTree);
var bTree = TreeFactory.CreateTree<int>(TreeType.BTree, 3); // B-Tree with degree 3
```

## Advanced Operations

### Path Finding
Get the path from root to a specific node:
```csharp
var path = tree.GetPathToNode(42);
```

### Level Operations
Get all nodes at a specific level:
```csharp
var nodes = tree.GetNodesAtLevel(2);
```

### Lowest Common Ancestor
Find the lowest common ancestor of two nodes:
```csharp
var lca = tree.GetLowestCommonAncestor(25, 75);
```

### Kth Elements
Get kth smallest or largest elements:
```csharp
var kthSmallest = tree.GetKthSmallest(3); // 3rd smallest
var kthLargest = tree.GetKthLargest(2);   // 2nd largest
```

### Range Operations
Get elements within a range:
```csharp
var range = tree.GetRange(10, 50); // Values between 10 and 50
```

## Performance Characteristics

| Operation | BST (Avg) | BST (Worst) | AVL Tree | Red-Black Tree | B-Tree |
|-----------|-----------|-------------|----------|----------------|--------|
| Search    | O(log n)  | O(n)        | O(log n)| O(log n)       | O(log_m n) |
| Insert    | O(log n)  | O(n)        | O(log n)| O(log n)       | O(log_m n) |
| Delete    | O(log n)  | O(n)        | O(log n)| O(log n)       | O(log_m n) |
| Space     | O(n)      | O(n)        | O(n)     | O(n)           | O(n)     |

Where:
- n = number of elements
- m = degree of B-Tree

## Examples

### Basic Usage
```csharp
using TreeDataStructure;

// Integer tree
var intTree = new BinarySearchTree<int>();
intTree.InsertRange(new[] { 50, 30, 70, 20, 40, 60, 80 });

// String tree
var stringTree = new BinarySearchTree<string>();
stringTree.InsertRange(new[] { "apple", "banana", "cherry", "date" });

// Custom comparable objects
public class Person : IComparable<Person>
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public int CompareTo(Person other)
    {
        return Age.CompareTo(other.Age);
    }
}

var personTree = new BinarySearchTree<Person>();
personTree.Insert(new Person { Name = "Alice", Age = 30 });
personTree.Insert(new Person { Name = "Bob", Age = 25 });
```

### Self-Balancing Trees
```csharp
// AVL Tree - strict balance
var avlTree = new AvlTree<int>();
avlTree.InsertRange(Enumerable.Range(1, 100)); // Always balanced

// Red-Black Tree - relaxed balance
var rbTree = new RedBlackTree<int>();
rbTree.InsertRange(Enumerable.Range(1, 100)); // Balanced with different strategy
```

### B-Tree for Large Datasets
```csharp
// B-Tree with configurable degree for disk optimization
var bTree = new BTree<int>(5); // Degree 5 - each node can hold up to 4 keys
bTree.InsertRange(Enumerable.Range(1, 1000));
```

### Advanced Traversals and Operations
```csharp
var tree = new BinarySearchTree<int>();
tree.InsertRange(new[] { 50, 30, 70, 20, 40, 60, 80 });

// Different traversals
var inOrder = tree.InOrderTraversal();        // 20, 30, 40, 50, 60, 70, 80
var preOrder = tree.PreOrderTraversal();      // 50, 30, 20, 40, 70, 60, 80
var postOrder = tree.PostOrderTraversal();    // 20, 40, 30, 60, 80, 70, 50
var levelOrder = tree.LevelOrderTraversal();  // 50, 30, 70, 20, 40, 60, 80
var reverseInOrder = tree.ReverseInOrderTraversal(); // 80, 70, 60, 50, 40, 30, 20

// Advanced operations
var min = tree.GetMin();                    // 20
var max = tree.GetMax();                    // 80
var thirdSmallest = tree.GetKthSmallest(3); // 40
var secondLargest = tree.GetKthLargest(2);  // 70
```

## Testing

The library includes comprehensive unit tests:

```bash
dotnet test tests/TreeDataStructure.Tests/TreeDataStructure.Tests.csproj
```

Tests cover:
- Basic operations for all tree types
- Edge cases and boundary conditions
- Performance validation
- Correctness of advanced operations
- Comparison between different tree implementations

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

Please ensure:
- All tests pass
- Code follows existing style
- New functionality is well-documented
- Performance considerations are addressed

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.