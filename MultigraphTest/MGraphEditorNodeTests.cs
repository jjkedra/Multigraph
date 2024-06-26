using Moq;
using MultigraphEditor.src.graph;

namespace MultigraphTest
{
    [TestClass]
    public class MGraphEditorNodeTests
    {
        [TestMethod]
        public void TestProperties()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            node.Label = "Test";
            Assert.AreEqual("Test", node.Label);
            Assert.IsNotNull(node.Edges);
        }

        [TestMethod]
        public void TestAddEdge()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            Mock<IEdge> edge = new Mock<IEdge>();
            node.AddEdge(edge.Object);
            Assert.IsTrue(node.Edges.Contains(edge.Object));
        }

        [TestMethod]
        public void TestRemoveEdge()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            Mock<IEdge> edge = new Mock<IEdge>();
            node.AddEdge(edge.Object);
            node.RemoveEdge(edge.Object);
            Assert.IsFalse(node.Edges.Contains(edge.Object));
        }

        [TestMethod]
        public void Constructor_SetsIdentifierAndLabel()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            Assert.AreEqual(node.Identifier.ToString(), node.Label);
        }

        [TestMethod]
        public void AddEdge_IncreasesEdgesCount()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            IEdge edge = new Mock<IEdge>().Object;
            node.AddEdge(edge);

            Assert.AreEqual(1, node.Edges.Count);
        }

        [TestMethod]
        public void RemoveEdge_DecreasesEdgesCount()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            IEdge edge = new Mock<IEdge>().Object;
            node.AddEdge(edge);
            node.RemoveEdge(edge);

            Assert.AreEqual(0, node.Edges.Count);
        }

        [TestMethod]
        public void GetIdentifier_ReturnsUniqueIdentifiersForDifferentInstances()
        {
            MGraphEditorNode node1 = new MGraphEditorNode();
            MGraphEditorNode node2 = new MGraphEditorNode();
            Assert.AreNotEqual(node1.Identifier, node2.Identifier);
        }

        [TestMethod]
        public void GetCoordinates_ReturnsCorrectCoordinates()
        {
            MGraphEditorNode node = new MGraphEditorNode { X = 100, Y = 150 };
            (float, float) coordinates = node.GetCoordinates();
            Assert.AreEqual((100f, 150f), coordinates);
        }

        [TestMethod]
        public void GetDrawingCoordinates_ReturnsAdjustedCoordinates()
        {
            MGraphEditorNode node = new MGraphEditorNode { X = 100, Y = 150, Diameter = 20 };
            (float, float) drawingCoordinates = node.GetDrawingCoordinates();
            Assert.AreEqual((90f, 140f), drawingCoordinates); // Adjusted by half the diameter
        }

        [TestMethod]
        public void IsInside_ReturnsTrueForPointInsideNode()
        {
            MGraphEditorNode node = new MGraphEditorNode { X = 100, Y = 100, Diameter = 20 };
            Assert.IsTrue(node.IsInside(105, 105)); // Point inside the node
        }

        [TestMethod]
        public void IsInside_ReturnsFalseForPointOutsideNode()
        {
            MGraphEditorNode node = new MGraphEditorNode { X = 100, Y = 100, Diameter = 20 };
            Assert.IsFalse(node.IsInside(50, 50), "Point outside the node");
        }

        [TestMethod]
        public void Equals_ReturnsTrueForSameInstance()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            Assert.IsTrue(node.Equals(node));
        }

        [TestMethod]
        public void Equals_ReturnsFalseForDifferentInstance()
        {
            MGraphEditorNode node1 = new MGraphEditorNode();
            MGraphEditorNode node2 = new MGraphEditorNode();
            Assert.IsFalse(node1.Equals(node2));
        }

        [TestMethod]
        public void NodeCounter_IncrementsCorrectlyAcrossInstances()
        {
            int initialCount = MGraphEditorNode.NodeCounter;

            _ = new MGraphEditorNode();
            Assert.AreEqual(initialCount + 1, MGraphEditorNode.NodeCounter);

            _ = new MGraphEditorNode();
            Assert.AreEqual(initialCount + 2, MGraphEditorNode.NodeCounter);
        }

        [TestMethod]
        public void RemoveEdge_NonExistentEdge_DoesNothing()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            IEdge edge1 = new Mock<IEdge>().Object;
            IEdge edge2 = new Mock<IEdge>().Object; // This edge won't be added

            node.AddEdge(edge1);
            node.RemoveEdge(edge2); // Attempt to remove an edge that was never added

            Assert.AreEqual(1, node.Edges.Count); // Edge count should remain unchanged
            Assert.IsTrue(node.Edges.Contains(edge1)); // The original edge should still be present
        }

        [TestMethod]
        public void AddEdge_DoesNotAllowDuplicates()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            IEdge edge = new Mock<IEdge>().Object;

            node.AddEdge(edge);
            node.AddEdge(edge); // Attempt to add the same edge again

            Assert.AreEqual(1, node.Edges.Count); // Duplicates are not allowed
        }

        [TestMethod]
        public void EachNode_HasUniqueIdentifier()
        {
            MGraphEditorNode node1 = new MGraphEditorNode();
            MGraphEditorNode node2 = new MGraphEditorNode();

            Assert.AreNotEqual(node1.Identifier, node2.Identifier, "Each node should have a unique identifier.");
        }

        [TestMethod]
        public void GetDrawingCoordinates_ReturnsExpectedValues()
        {
            MGraphEditorNode node = new MGraphEditorNode { X = 50, Y = 100, Diameter = 20 };
            int expectedX = 40; // Expected X is 50 - (20 / 2)
            int expectedY = 90; // Expected Y is 100 - (20 / 2)

            (float actualX, float actualY) = node.GetDrawingCoordinates();

            Assert.AreEqual(expectedX, actualX, "X coordinate is not correctly adjusted.");
            Assert.AreEqual(expectedY, actualY, "Y coordinate is not correctly adjusted.");
        }

        [TestMethod]
        public void Equals_ReturnsFalseForDifferentNodes()
        {
            MGraphEditorNode node1 = new MGraphEditorNode();
            MGraphEditorNode node2 = new MGraphEditorNode();

            Assert.IsFalse(node1.Equals(node2), "Equals should return false for different nodes.");
        }

        [TestMethod]
        public void IsInside_HandlesBoundaryConditionsCorrectly()
        {
            MGraphEditorNode node = new MGraphEditorNode { X = 50, Y = 50, Diameter = 20 };

            // Points on the edge of the node
            Assert.IsTrue(node.IsInside(50, 50), "Point on the boundary should be considered inside.");
            Assert.IsFalse(node.IsInside(30, 50), "Point just outside the boundary should be considered outside.");
            Assert.IsTrue(node.IsInside(60, 50), "Point on the opposite boundary should be considered inside.");
        }

        [TestMethod]
        public void GetHashCode_ReturnsDifferentValuesForDifferentNodes()
        {
            MGraphEditorNode node1 = new MGraphEditorNode();
            MGraphEditorNode node2 = new MGraphEditorNode();

            Assert.AreNotEqual(node1.GetHashCode(), node2.GetHashCode(), "Hash codes should be different for different nodes.");
        }

        [TestMethod]
        public void GetHashCode_ReturnsSameValueForSameNode()
        {
            MGraphEditorNode node1 = new MGraphEditorNode();
            MGraphEditorNode node2 = node1;

            Assert.AreEqual(node1.GetHashCode(), node2.GetHashCode(), "Hash codes should be the same for the same node.");
        }

        [TestMethod]
        public void GetHashCode_ReturnsExpectedValue()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            int expectedHashCode = node.Identifier.GetHashCode();

            Assert.AreNotEqual(expectedHashCode, node.GetHashCode(), "Hash code is not as expected.");
        }

        [TestMethod]
        public void Equals_ReturnsTrueForSameNode()
        {
            MGraphEditorNode node = new MGraphEditorNode();

            Assert.IsTrue(node.Equals(node), "Node should be equal to itself.");
        }

        [TestMethod]
        public void Equals_ReturnsFalseForDifferentType()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            MGraphEditorEdge edge = new MGraphEditorEdge();

            Assert.IsFalse(node.Equals(edge), "Node should not be equal to an edge.");
        }

        [TestMethod]
        public void Equals_ReturnsFalseForNull()
        {
            MGraphEditorNode node = new MGraphEditorNode();

            Assert.IsFalse(node.Equals(null), "Node should not be equal to null.");
        }

        [TestMethod]
        public void Equals_ReturnsFalseForDifferentNode()
        {
            MGraphEditorNode node1 = new MGraphEditorNode();
            MGraphEditorNode node2 = new MGraphEditorNode();

            Assert.IsFalse(node1.Equals(node2), "Different nodes should not be equal.");
        }

        [TestMethod]
        public void Clone_CreatesNewInstance()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            IMGraphEditorNode clonedNode = node.Clone();

            Assert.AreEqual(node, clonedNode, "Cloned node should be a new instance.");
        }

        [TestMethod]
        public void Clone_CopiesProperties()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            IMGraphEditorNode clonedNode = node.Clone();

            Assert.AreEqual(node.Label, clonedNode.Label, "Label is not copied correctly.");
            Assert.AreEqual(node.X, clonedNode.X, "X coordinate is not copied correctly.");
            Assert.AreEqual(node.Y, clonedNode.Y, "Y coordinate is not copied correctly.");
            Assert.AreEqual(node.Diameter, clonedNode.Diameter, "Diameter is not copied correctly.");
        }

        [TestMethod]
        public void Clone_CopiesEdges()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            MGraphEditorEdge edge = new MGraphEditorEdge();
            node.AddEdge(edge);

            IMGraphEditorNode clonedNode = node.Clone();

            Assert.IsTrue(clonedNode.Edges.Contains(edge), "Edges are not copied correctly.");
        }

        [TestMethod]
        public void Clone_CopiesIdentifier()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            IMGraphEditorNode clonedNode = node.Clone();

            Assert.AreEqual(node.Identifier, clonedNode.Identifier, "Identifier is not copied correctly.");
        }

        [TestMethod]
        public void Clone_CopiesCoordinates()
        {
            MGraphEditorNode node = new MGraphEditorNode { X = 50, Y = 100 };
            IMGraphEditorNode clonedNode = node.Clone();

            Assert.AreEqual(node.X, clonedNode.X, "X coordinate is not copied correctly.");
            Assert.AreEqual(node.Y, clonedNode.Y, "Y coordinate is not copied correctly.");
        }

        [TestMethod]
        public void Clone_CopiesDiameter()
        {
            MGraphEditorNode node = new MGraphEditorNode { Diameter = 20 };
            IMGraphEditorNode clonedNode = node.Clone();

            Assert.AreEqual(node.Diameter, clonedNode.Diameter, "Diameter is not copied correctly.");
        }

        [TestMethod]
        public void Clone_CopiesEdgesCorrectly()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            MGraphEditorEdge edge = new MGraphEditorEdge();
            node.AddEdge(edge);

            IMGraphEditorNode clonedNode = node.Clone();

            Assert.IsTrue(clonedNode.Edges.Contains(edge), "Edges are not copied correctly.");
        }

        [TestMethod]
        public void Clone_CopiesEdgesButNotSameInstance()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            MGraphEditorEdge edge = new MGraphEditorEdge();
            node.AddEdge(edge);

            IMGraphEditorNode clonedNode = node.Clone();

            Assert.AreNotSame(node.Edges, clonedNode.Edges, "Cloned edges should be a new instance.");
        }

        [TestMethod]
        public void Clone_CopiesEdgesButNotSameEdgeInstance()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            MGraphEditorEdge edge = new MGraphEditorEdge();
            node.AddEdge(edge);

            IMGraphEditorNode clonedNode = node.Clone();

            Assert.AreNotSame(node.Edges[0], clonedNode.Edges[0], "Cloned edge should be a new instance.");
        }

        [TestMethod]
        public void Clone_CopiesEdgesButNotSameEdgeInstance2()
        {
            MGraphEditorNode node = new MGraphEditorNode();
            MGraphEditorEdge edge = new MGraphEditorEdge();
            node.AddEdge(edge);

            IMGraphEditorNode clonedNode = node.Clone();

            Assert.AreNotSame(node.Edges[0], clonedNode.Edges[0], "Cloned edge should be a new instance.");
        }
    }
}