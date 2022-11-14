using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ValidationProviders;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.UnitTests.TestsBLValidation
{
    /// <summary>
    /// Testing the correctness of passed objects Area to business logic methods.
    /// </summary>
    [TestFixture]
	public class AreaValidationTest
	{
        private const string MESSAGE_EXCEPTION_DESCRIPTION = "Invalid Description: Description does not empty and longer then 200 characters."; 
        private const string MESSAGE_EXCEPTION_COORD_X = "Invalid CoordX: CoordX cannot be less 1.";
        private const string MESSAGE_EXCEPTION_COORD_Y = "Invalid CoordY: CoordY cannot be less 1.";
        private const string MESSAGE_EXCEPTION_LAYOUT_ID = "Invalid LayoutId: such layout does not exist.";
        private const string MESSAGE_EXCEPTION_AREA_ID = "Area id cannot be 0 or less.";
        private const string MESSAGE_EXCEPTION_ADD_NULL = "Can not add null area. (Parameter 'area')";
        private const string MESSAGE_EXCEPTION_ID = "Invalid Id: Id cannot be less 1.";
        private const string MESSAGE_EXCEPTION_UPDATE_NULL = "Can not update null area. (Parameter 'area')";

        [Test]
		public async Task AreaGetAll_WhenHaveCollection_ShouldReturnArea()
		{
            // Arrange
            var listArea = new List<Area>
            {
                new Area { Id = 3, LayoutId = 1, Description = "Normal", CoordX = 2, CoordY = 3 },
                new Area { Id = 4, LayoutId = 1, Description = "Very well", CoordX = 3, CoordY = 3 },
                new Area { Id = 6, LayoutId = 2, Description = "Bad", CoordX = 4, CoordY = 4 },
            };
            var areaServiceMock = new Mock<IServiceBase<Area>>();
            areaServiceMock.Setup(m => m.GetAllAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(listArea.Where(x => x.LayoutId == 1)));

            var areaService = areaServiceMock.Object;

            // Act
            var actualList = await areaService.GetAllAsync(1);
            listArea.RemoveAt(2);

            // Assert
            CollectionAssert.AreEquivalent(listArea, actualList);
        }

        [Test]
        public void Add_WhenAreaIsNull_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => 
                AreaValidationProvider.IsValidToAdd(null), MESSAGE_EXCEPTION_ADD_NULL);
        }

        [Test]
        public void AddUpdate_WhenAreaLayoutIdFailed_ReturnException()
        {
            // Arrange
            var area = new Area { Id = 1, LayoutId = -4, CoordX = 1, CoordY = 2, Description = "Well" };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => AreaValidationProvider.IsValidToAdd(area), MESSAGE_EXCEPTION_LAYOUT_ID);
            Assert.Throws<ArgumentException>(() => AreaValidationProvider.IsValidToUpdate(area), MESSAGE_EXCEPTION_LAYOUT_ID);
        }

        [Test]
        public void AddUpdate_WhenAreaDescriptionFailed_ReturnException()
        {
            // Arrange
            var area = new Area { Id = 1, LayoutId = 1, CoordX = 1, CoordY = 2, Description = "  " };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => AreaValidationProvider.IsValidToAdd(area), MESSAGE_EXCEPTION_DESCRIPTION);
            Assert.Throws<ArgumentException>(() => AreaValidationProvider.IsValidToUpdate(area), MESSAGE_EXCEPTION_DESCRIPTION);
        }

        [Test]
        public void AddUpdate_WhenAreaCoordsFailed_ReturnException()
        {
            // Arrange
            var areaX = new Area { Id = 1, LayoutId = 1, CoordX = -1, CoordY = 2, Description = "Error X" };
            var areaY = new Area { Id = 1, LayoutId = 1, CoordX = 1, CoordY = -2, Description = "Error Y" };        

            // Act, Assert
            Assert.Throws<ArgumentException>(() => AreaValidationProvider.IsValidToAdd(areaX), MESSAGE_EXCEPTION_COORD_X);
            Assert.Throws<ArgumentException>(() => AreaValidationProvider.IsValidToAdd(areaY), MESSAGE_EXCEPTION_COORD_Y);
            Assert.Throws<ArgumentException>(() => AreaValidationProvider.IsValidToUpdate(areaX), MESSAGE_EXCEPTION_COORD_X);
            Assert.Throws<ArgumentException>(() => AreaValidationProvider.IsValidToUpdate(areaY), MESSAGE_EXCEPTION_COORD_Y);
        }

        [Test]
        public async Task Add_WhenAreaValidationProvider_AddAndReturnTrue()
        {
            // Arrange
            var areaList = new List<Area>();
            var areaServiceMock = new Mock<IServiceBase<Area>>();
            areaServiceMock.Setup(m => m.AddAsync(It.IsAny<Area>())).Callback<Area>(area => areaList.Add(area))
                .Returns(Task.FromResult(true));

            var area = new Area { LayoutId = 1, CoordX = 2, CoordY = 2, Description = "Very Well" };

            // Act
            bool result = await areaServiceMock.Object.AddAsync(area);

            // Assert
            Assert.IsTrue(result);
            CollectionAssert.AreEquivalent(areaList, new List<Area> { area });
        }

        [Test]
        public void Update_WnenAreaIsNull_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => AreaValidationProvider.IsValidToUpdate(null), MESSAGE_EXCEPTION_UPDATE_NULL);
        }

        [Test]
        public void Update_WhenAreaIdNotValidly_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => 
                AreaValidationProvider.IsValidToUpdate(new Area { Id = -3 }), MESSAGE_EXCEPTION_ID);
        }

        [Test]
        public async Task Update_WhenAreaValidationProviderly_UpdateAndReturnTrue()
        {
            // Arrange
            var area = new Area { Id = 1, LayoutId = 2, CoordX = 1, CoordY = 1, Description = "RRR" };
            var areaList = new List<Area> { area };

            var areaServiceMock = new Mock<IServiceBase<Area>>();
            areaServiceMock.Setup(m => m.UpdateAsync(It.IsAny<Area>())).Callback<Area>(area =>
            {
                areaList.RemoveAt(0);
                areaList.Add(area);
            }).Returns(Task.FromResult(true));

            // Act
            area.CoordY = 101;
            bool result = await areaServiceMock.Object.UpdateAsync(area);

            // Assert
            Assert.IsTrue(result);
            CollectionAssert.AreEquivalent(areaList, new List<Area> { area });
        }

        [Test]
        public void Delete_WhenAreaIdNotValidly_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => AreaValidationProvider.IsValidToDelete(-3), MESSAGE_EXCEPTION_AREA_ID);
        }

        [Test]
        public async Task Delete_WnenAreaValidationProviderly_DeleteAndReturnTrue()
        {
            // Arrange
            var area = new Area { Id = 2, LayoutId = 2, CoordX = 1, CoordY = 1, Description = "Hey" };
            var areaList = new List<Area> { area };

            var areaServiceMock = new Mock<IServiceBase<Area>>();
            areaServiceMock.Setup(m => m.DeleteAsync(It.IsAny<int>()))
                .Callback<int>(area => areaList.RemoveAt(area)).Returns(Task.FromResult(true));

            // Act
            bool result = await areaServiceMock.Object.DeleteAsync(0);

            // Assert
            Assert.IsTrue(result);
            Assert.IsEmpty(areaList);
        }
    }
}
