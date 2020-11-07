using System;
using System.Collections.Generic;
using System.Linq;
using Tree = MetricSpace.Float._2.Euclidean.KdTree<string>;
using Node = MetricSpace.Float._2.Euclidean.KdTree<string>.Node;
using Xunit;
using MetricSpace;

namespace XUnitTestMetricSpace
{
    struct City
    {
        public string Address;
        public float Lat;
        public float Long;
        public float DistanceFromToowoomba;
    }

    public class KdTreeTests
    {
        private Tree tree;

        public KdTreeTests()
        {
            tree = new Tree();

            testNodes = new List<Node>();
            testNodes.AddRange(new Node[]
            {
                new Node((5, 5), "Root"),

                new Node((2.5f, 2.5f), "Root-Left"),
                new Node((7.5f, 7.5f), "Root-Right"),
                new Node((1, 10), "Root-Left-Left"),
                new Node((10, 10), "Root-Right-Right")
            });
        }

        private readonly List<Node> testNodes;

        private void AddTestNodes()
        {
            foreach (var node in testNodes)
                if (!tree.Add(node.Point, node.Value))
                    throw new Exception("Failed to add node to tree");
        }

        [Fact]
        public void TestAdd()
        {
            // Add nodes to tree
            AddTestNodes();

            // Check count of nodes is right
            Assert.Equal(testNodes.Count, tree.Count);
        }

        [Fact]
        public void TestAddDuplicateInSkipMode()
        {
            tree = new Tree();

            Assert.Equal(AddDuplicateBehavior.Skip, tree.AddDuplicateBehavior);

            AddTestNodes();

            var count = tree.Count;

            var added = tree.Add(testNodes[0].Point, "Some other value");

            Assert.False(added);
            Assert.Equal(count, tree.Count);
        }

        [Fact]
        public void TestAddDuplicateInErrorMode()
        {
            tree = new Tree(AddDuplicateBehavior.Error);

            AddTestNodes();

            var count = tree.Count;
            Exception? error = null;

            try
            {
                tree.Add(testNodes[0].Point, "Some other value");
            }
            catch (Exception e)
            {
                error = e;
            }

            Assert.Equal(count, tree.Count);
            Assert.NotNull(error);
            Assert.True(error is DuplicateNodeError);
        }

        [Fact]
        public void TestAddDuplicateInUpdateMode()
        {
            tree = new Tree(AddDuplicateBehavior.Update);

            AddTestNodes();

            var newValue = "I love chicken, I love liver, Meow Mix Meow Mix please deliver";

            tree.Add(testNodes[0].Point, newValue);

            var actualValue = tree.FindValueAt(testNodes[0].Point);

            Assert.Equal(newValue, actualValue);
        }

        [Fact]
        public void TestTryFindValueAt()
        {
            AddTestNodes();

            string actualValue;

            foreach (var node in testNodes)
            {
                if (tree.TryFindValueAt(node.Point, out actualValue))
                    Assert.Equal(node.Value, actualValue);
                else
                    throw new Exception("Could not find test node");
            }

            if (!tree.TryFindValueAt((3.14f, 5), out actualValue))
                Assert.Null(actualValue);
            else
                throw new Exception("Reportedly found node it shouldn't have");
        }

        [Fact]
        public void TestFindValueAt()
        {
            AddTestNodes();

            string actualValue;

            foreach (var node in testNodes)
            {
                actualValue = tree.FindValueAt(node.Point);

                Assert.Equal(node.Value, actualValue);
            }

            actualValue = tree.FindValueAt((3.15f, 5));

            Assert.Null(actualValue);
        }

        [Fact]
        public void TestFindValue()
        {
            AddTestNodes();

            Fixed2<float>.Array actualPoint;

            foreach (var node in testNodes)
            {
                actualPoint = tree.FindValue(node.Value);
                Assert.Equal(node.Point, actualPoint);
            }

            var success = tree.TryFindValue("Your Mumma", out _);
            Assert.False(success);
        }

        [Fact]
        public void TestRemoveAt()
        {
            AddTestNodes();

            var nodesToRemove = new Node[] {
                testNodes[1], // Root-Left
				testNodes[0] // Root
			};

            foreach (var nodeToRemove in nodesToRemove)
            {
                tree.RemoveAt(nodeToRemove.Point);
                testNodes.Remove(nodeToRemove);

                Assert.False(tree.TryFindValue(nodeToRemove.Value, out _));
                Assert.False(tree.TryFindValueAt(nodeToRemove.Point, out _));

                foreach (var testNode in testNodes)
                {
                    Assert.Equal(testNode.Value, tree.FindValueAt(testNode.Point));
                    Assert.Equal(testNode.Point, tree.FindValue(testNode.Value));
                }

                Assert.Equal(testNodes.Count, tree.Count);
            }
        }

        [Fact]
        public void TestGetNearestNeighbours()
        {
            var toowoomba = new City()
            {
                Address = "Toowoomba, QLD, Australia",
                Lat = -27.5829487f,
                Long = 151.8643252f,
                DistanceFromToowoomba = 0
            };

            City[] cities = new City[]
            {
                toowoomba,
                new City()
                {
                    Address = "Brisbane, QLD, Australia",
                    Lat = -27.4710107f,
                    Long = 153.0234489f,
                    DistanceFromToowoomba = 1.16451615177537f
                },
                new City()
                {
                    Address = "Goldcoast, QLD, Australia",
                    Lat = -28.0172605f,
                    Long = 153.4256987f,
                    DistanceFromToowoomba = 1.6206523211724f
                },
                new City()
                {
                    Address = "Sunshine, QLD, Australia",
                    Lat = -27.3748288f,
                    Long = 153.0554193f,
                    DistanceFromToowoomba = 1.20913979664506f
                },
                new City()
                {
                    Address = "Melbourne, VIC, Australia",
                    Lat = -37.814107f,
                    Long = 144.96328f,
                    DistanceFromToowoomba = 12.3410301438779f
                },
                new City()
                {
                    Address = "Sydney, NSW, Australia",
                    Lat = -33.8674869f,
                    Long = 151.2069902f,
                    DistanceFromToowoomba = 6.31882185929341f
                },
                new City()
                {
                    Address = "Perth, WA, Australia",
                    Lat = -31.9530044f,
                    Long = 115.8574693f,
                    DistanceFromToowoomba = 36.2710774395312f
                },
                new City()
                {
                    Address = "Darwin, NT, Australia",
                    Lat = -12.4628198f,
                    Long = 130.8417694f,
                    DistanceFromToowoomba = 25.895292049265f
                }
				/*,
				new City()
				{
					Address = "London, England",
					Lat = 51.5112139f,
					Long = -0.1198244f,
					DistanceFromToowoomba = 171.33320836029f
					
				}*/
			};

            foreach (var city in cities)
            {
                tree.Add((city.Long, -city.Lat), city.Address);
            }

            /*
			var sb = new System.Text.StringBuilder();
			sb.AppendLine("Before Balance:");
			sb.AppendLine(tree.ToString());
			sb.AppendLine("");
			sb.AppendLine("");
			tree.Balance();
			sb.AppendLine("After Balance:");
			sb.AppendLine(tree.ToString());
			System.Windows.Forms.Clipboard.SetText(sb.ToString());
			*/

            for (var findLimit = 0; findLimit <= cities.Length; findLimit++)
            {
                var actualNeighbours = tree.GetNearestNeighbours(
                    (toowoomba.Long, -toowoomba.Lat),
                    findLimit);

                var expectedNeighbours = cities
                    .OrderBy(p => p.DistanceFromToowoomba)
                    .Take(findLimit)
                    .ToArray();

                Assert.Equal(findLimit, actualNeighbours.Length);
                Assert.Equal(findLimit, expectedNeighbours.Length);

                for (var index = 0; index < actualNeighbours.Length; index++)
                {
                    Assert.Equal(expectedNeighbours[index].Address, actualNeighbours[index].Value);
                }
            }
        }

        [Fact]
        public void TestRadialSearch()
        {
            var toowoomba = new City()
            {
                Address = "Toowoomba, QLD, Australia",
                Lat = -27.5829487f,
                Long = 151.8643252f,
                DistanceFromToowoomba = 0
            };

            City[] cities = new City[]
            {
                toowoomba,
                new City()
                {
                    Address = "Brisbane, QLD, Australia",
                    Lat = -27.4710107f,
                    Long = 153.0234489f,
                    DistanceFromToowoomba = 1.16451615177537f
                },
                new City()
                {
                    Address = "Goldcoast, QLD, Australia",
                    Lat = -28.0172605f,
                    Long = 153.4256987f,
                    DistanceFromToowoomba = 1.6206523211724f
                },
                new City()
                {
                    Address = "Sunshine, QLD, Australia",
                    Lat = -27.3748288f,
                    Long = 153.0554193f,
                    DistanceFromToowoomba = 1.20913979664506f
                },
                new City()
                {
                    Address = "Melbourne, VIC, Australia",
                    Lat = -37.814107f,
                    Long = 144.96328f,
                    DistanceFromToowoomba = 12.3410301438779f
                },
                new City()
                {
                    Address = "Sydney, NSW, Australia",
                    Lat = -33.8674869f,
                    Long = 151.2069902f,
                    DistanceFromToowoomba = 6.31882185929341f
                },
                new City()
                {
                    Address = "Perth, WA, Australia",
                    Lat = -31.9530044f,
                    Long = 115.8574693f,
                    DistanceFromToowoomba = 36.2710774395312f
                },
                new City()
                {
                    Address = "Darwin, NT, Australia",
                    Lat = -12.4628198f,
                    Long = 130.8417694f,
                    DistanceFromToowoomba = 25.895292049265f
                }
            };

            foreach (var city in cities)
            {
                tree.Add((city.Long, -city.Lat), city.Address);
            }
            var expectedNeighbours = cities
                .OrderBy(p => p.DistanceFromToowoomba).ToList();

            for (var i = 1; i < 100; i *= 2)
            {
                var actualNeighbours = tree.RadialSearch((toowoomba.Long, -toowoomba.Lat), i);

                var list = Tree.CreateUnlimitedList();
                tree.RadialSearch((toowoomba.Long, -toowoomba.Lat), i, list);
                var sorted = list.GetSortedArray();

                for (var index = 0; index < actualNeighbours.Length; index++)
                {
                    Assert.Equal(expectedNeighbours[index].Address, actualNeighbours[index].Value);
                    Assert.Equal(expectedNeighbours[index].Address, sorted[index].Value);
                }
            }
        }

        [Fact]
        public void TestEnumerable()
        {
            AddTestNodes();

            foreach (var node in tree)
            {
                var testNode = testNodes.FirstOrDefault(n => n.Point.Item1 == node.Point.Item1 && n.Point.Item2 == node.Point.Item2 && n.Value == node.Value);

                Assert.NotNull(testNode);

                testNodes.Remove(testNode);
            }

            Assert.Empty(testNodes);
        }
    }
}
