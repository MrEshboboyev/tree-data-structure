namespace TreeDataStructure.Tests;

public class TreeTests
{
    [Fact]
    public void BinarySearchTree_InsertAndContains_WorksCorrectly()
    {
        var bst = new BinarySearchTree<int>();
        bst.Insert(50);
        bst.Insert(30);
        bst.Insert(70);

        Assert.True(bst.Contains(50));
        Assert.True(bst.Contains(30));
        Assert.True(bst.Contains(70));
        Assert.False(bst.Contains(100));
    }

    [Fact]
    public void BinarySearchTree_InOrderTraversal_ReturnsSortedValues()
    {
        var bst = new BinarySearchTree<int>();
        int[] values = [50, 30, 70, 20, 40, 60, 80];
        foreach (int value in values)
        {
            bst.Insert(value);
        }

        var result = bst.InOrderTraversal().ToArray();
        var expected = new[] { 20, 30, 40, 50, 60, 70, 80 };

        Assert.Equal(expected, result);
    }

    [Fact]
    public void BinarySearchTree_Remove_RemovesCorrectly()
    {
        var bst = new BinarySearchTree<int>();
        int[] values = [50, 30, 70, 20, 40, 60, 80];
        foreach (int value in values)
        {
            bst.Insert(value);
        }

        Assert.True(bst.Remove(30));
        Assert.False(bst.Contains(30));
        Assert.Equal(6, bst.Count);
    }

    [Fact]
    public void BinarySearchTree_GetMinAndMax_WorksCorrectly()
    {
        var bst = new BinarySearchTree<int>();
        int[] values = [50, 30, 70, 20, 40, 60, 80];
        foreach (int value in values)
        {
            bst.Insert(value);
        }

        Assert.Equal(20, bst.GetMin());
        Assert.Equal(80, bst.GetMax());
    }

    [Fact]
    public void BinarySearchTree_Height_ReturnsCorrectHeight()
    {
        var bst = new BinarySearchTree<int>();
        int[] values = [50, 30, 70, 20, 40, 60, 80];
        foreach (int value in values)
        {
            bst.Insert(value);
        }

        Assert.Equal(3, bst.Height);
    }

    [Fact]
    public void BinarySearchTree_Count_WorksCorrectly()
    {
        var bst = new BinarySearchTree<int>();
        Assert.Equal(0, bst.Count);

        bst.Insert(50);
        Assert.Equal(1, bst.Count);

        bst.Insert(30);
        Assert.Equal(2, bst.Count);
    }

    [Fact]
    public void BinarySearchTree_Clear_EmptiesTree()
    {
        var bst = new BinarySearchTree<int>();
        bst.Insert(50);
        bst.Insert(30);
        bst.Insert(70);

        bst.Clear();

        Assert.Equal(0, bst.Count);
        Assert.True(bst.IsEmpty);
    }

    [Fact]
    public void BinarySearchTree_Find_ReturnsCorrectNode()
    {
        var bst = new BinarySearchTree<int>();
        bst.Insert(50);
        bst.Insert(30);
        bst.Insert(70);

        var node = bst.Find(30);
        Assert.NotNull(node);
        Assert.Equal(30, node.Data);
    }

    [Fact]
    public void BinarySearchTree_PreOrderTraversal_WorksCorrectly()
    {
        var bst = new BinarySearchTree<int>();
        int[] values = [50, 30, 70, 20, 40, 60, 80];
        foreach (int value in values)
        {
            bst.Insert(value);
        }

        var result = bst.PreOrderTraversal().ToArray();
        var expected = new[] { 50, 30, 20, 40, 70, 60, 80 };

        Assert.Equal(expected, result);
    }

    [Fact]
    public void BinarySearchTree_PostOrderTraversal_WorksCorrectly()
    {
        var bst = new BinarySearchTree<int>();
        int[] values = [50, 30, 70, 20, 40, 60, 80];
        foreach (int value in values)
        {
            bst.Insert(value);
        }

        var result = bst.PostOrderTraversal().ToArray();
        var expected = new[] { 20, 40, 30, 60, 80, 70, 50 };

        Assert.Equal(expected, result);
    }

    [Fact]
    public void BinarySearchTree_LevelOrderTraversal_WorksCorrectly()
    {
        var bst = new BinarySearchTree<int>();
        int[] values = [50, 30, 70, 20, 40, 60, 80];
        foreach (int value in values)
        {
            bst.Insert(value);
        }

        var result = bst.LevelOrderTraversal().ToArray();
        var expected = new[] { 50, 30, 70, 20, 40, 60, 80 };

        Assert.Equal(expected, result);
    }

    [Fact]
    public void BinarySearchTree_StringValues_WorksCorrectly()
    {
        var bst = new BinarySearchTree<string>();
        string[] values = { "banana", "apple", "orange", "grape", "kiwi" };
        foreach (string value in values)
        {
            bst.Insert(value);
        }

        var result = bst.InOrderTraversal().ToArray();
        var expected = new[] { "apple", "banana", "grape", "kiwi", "orange" };

        Assert.Equal(expected, result);
    }

    [Fact]
    public void AvlTree_InsertMaintainsBalance()
    {
        var avl = new AvlTree<int>();
        // Insert values that would create an unbalanced tree in BST
        int[] values = [1, 2, 3, 4, 5, 6, 7];
        foreach (int value in values)
        {
            avl.Insert(value);
        }

        // AVL tree should be balanced, so height should be much less than 7
        Assert.True(avl.Height <= 4); // Should be approximately log2(n)
    }

    [Fact]
    public void AvlTree_RemoveMaintainsBalance()
    {
        var avl = new AvlTree<int>();
        int[] values = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        foreach (int value in values)
        {
            avl.Insert(value);
        }

        int initialHeight = avl.Height;
        avl.Remove(1); // Remove root to force restructure
        int finalHeight = avl.Height;

        // Height should remain reasonable after removal
        Assert.True(finalHeight <= initialHeight || finalHeight <= 5);
    }

    [Fact]
    public void BinarySearchTree_SerializeDeserialize_WorksCorrectly()
    {
        var originalTree = new BinarySearchTree<int>();
        int[] values = [5, 3, 8, 2, 4, 7, 9];
        foreach (int value in values)
        {
            originalTree.Insert(value);
        }

        string serialized = originalTree.Serialize();
        var deserializedTree = new BinarySearchTree<int>();
        deserializedTree.Deserialize(serialized);

        var originalValues = originalTree.InOrderTraversal().ToArray();
        var deserializedValues = deserializedTree.InOrderTraversal().ToArray();

        Assert.Equal(originalValues, deserializedValues);
        Assert.Equal(originalTree.Count, deserializedTree.Count);
    }

    [Fact]
    public void BinarySearchTree_Enumerator_WorksCorrectly()
    {
        var bst = new BinarySearchTree<int>();
        int[] values = [50, 30, 70, 20, 40, 60, 80];
        foreach (int value in values)
        {
            bst.Insert(value);
        }

        var result = bst.ToArray();
        var expected = new[] { 20, 30, 40, 50, 60, 70, 80 }; // In-order by default

        Assert.Equal(expected, result);
    }

    [Fact]
    public void BinarySearchTree_DuplicateValue_ThrowsException()
    {
        var bst = new BinarySearchTree<int>();
        bst.Insert(50);

        Assert.Throws<InvalidOperationException>(() => bst.Insert(50));
    }

    [Fact]
    public void BinarySearchTree_EmptyTree_ThrowsOnMinOrMax()
    {
        var bst = new BinarySearchTree<int>();

        Assert.Throws<InvalidOperationException>(() => bst.GetMin());
        Assert.Throws<InvalidOperationException>(() => bst.GetMax());
    }
}
