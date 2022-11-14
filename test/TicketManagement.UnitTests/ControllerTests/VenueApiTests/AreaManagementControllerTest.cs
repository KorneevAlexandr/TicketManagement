using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.ClientModels.Areas;

namespace TicketManagement.UnitTests.ControllerTests.VenueApiTests
{
	/// <summary>
	/// Locates unit tests for the VenueApi AreaManagementController.
	/// </summary>
	public class AreaManagementControllerTest : VenueControllerUnitTestBase
	{
		[Test]
		public async Task GetAreas_FromValidLayoutId_ReturnLayoutsModel()
		{
			// Arrange
			var layout = new LayoutDto { Id = 1 };
			_mockAreaService.Setup(method => method.GetAllAsync(layout.Id))
				.ReturnsAsync(new List<AreaDto>());
			
			// Act
			var result = await _areaController.GetAreas(layout.Id);

			// Assert
			result.Should().BeOfType<JsonResult>();
		}

		[Test]
		public async Task GetArea_FromGettingModel_ReturnModel()
		{
			// Arrange
			var expectedModelArea = new AreaInfoModel
			{
				Id = 1,
				Description = "Hey!",
				CoordX = 12,
				CoordY = 4,
			};
			_mockAreaService.Setup(method => method.GetAsync(It.IsAny<int>()))
				.ReturnsAsync(new AreaDto
				{
					Id = expectedModelArea.Id,
					Description = expectedModelArea.Description,
					CoordX = expectedModelArea.CoordX,
					CoordY = expectedModelArea.CoordY,
				});

			// Act
			var jsonResult = await _areaController.GetArea(expectedModelArea.Id);

			// Assert
			var result = (AreaInfoModel)((JsonResult)jsonResult).Value;
			expectedModelArea.Should().BeEquivalentTo(result);
		}

		[Test]
		public async Task CreateArea_ValidAreaModel_AddedArea()
		{
			// Arrange
			var expectedAreas = new List<AreaDto>();
			var areaDto = new AreaDto { LayoutId = 1, CoordX = 1, CoordY = 2, Description = "Desc", };
			var addedArea = new AreaInfoModel
			{
				LayoutId = 1,
				CoordX = 1,
				CoordY = 2,
				Description = "Desc",
			};
			_mockAreaService.Setup(method => method.AddAsync(It.IsAny<AreaDto>()))
				.Callback<AreaDto>(area => expectedAreas.Add(areaDto));
			_mockAreaService.Setup(method => method.GetAllAsync(It.IsAny<int>()))
				.ReturnsAsync(expectedAreas);

			// Act
			await _areaController.CreateArea(addedArea);

			// Assert
			expectedAreas.Should().NotBeEmpty();
		}

		[Test]
		public async Task DeleteArea_AreaIdValid_DeletedArea()
		{
			// Arrange
			var areaId = 1;
			var areas = new List<AreaDto> { new AreaDto { Id = areaId, } };
			_mockAreaService.Setup(method => method.DeleteAsync(areaId))
				.Callback<int>(id => areas.Remove(areas.FirstOrDefault(area => area.Id == id)));

			// Act
			await _areaController.DeleteArea(areaId);

			// Assert
			areas.Should().BeEmpty();
		}

		[Test]
		public async Task UpdateArea_AreaModelValid_ReturnOkResult()
		{
			// Arrange
			var area = new AreaDto
			{
				Id = 1,
				Description = "Desc",
				CoordX = 1,
				CoordY = 1,
				LayoutId = 1,
			};
			var areaModel = new AreaInfoModel
			{
				Id = 1,
				Description = "Desc",
				CoordX = 1,
				CoordY = 1,
				LayoutId = 1,
			};
			_mockAreaService.Setup(method => method.GetAsync(area.Id)).ReturnsAsync(area);

			// Act
			var result = await _areaController.UpdateArea(areaModel);

			// Assert
			result.Should().BeOfType<OkResult>();
		}
	}
}
