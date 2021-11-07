$(document).ready(function () {
	var host = $(location).attr('host');
	var protocol = $(location).attr('protocol');
	var pathName = $(location).attr('pathname');

	

	

	if (pathName.includes("add-employee")) {
		$("#selectRole").prop("readonly", true);
		$("#selectRegion").prop("readonly", true);
		$("#selectCity").prop("readonly", true);

		$.ajax({
			type: 'GET',
			dataType: 'json',
			url: protocol + '//' + host + '/api/countries/get-countries',
			success: function (data) {
				$("#selectCountry").prop("readonly", false);
				if (!pathName.includes("edit-employee")) {
					var countries = JSON.parse(JSON.stringify(data));
					for (var item in countries) {
						$("#selectCountry").append('<option value="' + countries[item].CountryName + '">' + countries[item].CountryName + "</option>");
					}
				}
			}
		});

		$("#selectCountry").change(function () {
			var countryName = $(this).val();
			console.log(countryName);

			$.ajax({
				type: 'GET',
				dataType: 'json',
				url: protocol + '//' + host + '/api/regions/get-regions-for-country/' + countryName,
				success: function (data) {
					$("#regions").find('option').remove();
					$("#selectRegion").prop("readonly", false);

					var regions = JSON.parse(JSON.stringify(data));
					for (var item in regions) {
						$("#regions").append('<option value="' + regions[item].RegionName + '">');
					}
				}
			});
		});


		$("#selectRegion").change(function () {
			var regionName = $(this).val();

			$.ajax({
				type: 'GET',
				dataType: 'json',
				url: protocol + '//' + host + '/api/cities/get-cities-for-region/' + regionName,
				success: function (data) {
					$("#cities").find('option').remove();
					$("#selectCity").prop("readonly", false);
					$("#inputPostalCode").val('');

					if (data != undefined) {
						var cities = JSON.parse(JSON.stringify(data));
						for (var item in cities) {
							$("#cities").append('<option value="' + cities[item].CityName + '">');
						}
					} else {
						$("#selectCity").val('');
					}
				}
			});
		});

		$("#selectCity").change(function () {
			var cityName = $(this).val();

			$.ajax({
				type: 'GET',
				dataType: 'json',
				url: protocol + '//' + host + '/api/postal-codes/get-postal-code-for-city/' + cityName,
				success: function (data) {
					if (data != undefined) {
						var postalCode = JSON.parse(JSON.stringify(data));
						console.log(postalCode.PostalCode);
						$("#inputPostalCode").val(postalCode.PostalCode);
						$("#inputPostalCode").prop("readonly", true);
					} else {
						$("#inputPostalCode").val('');
						$("#inputPostalCode").prop("readonly", false);
					}
				}
			});
		});

		$("#selectRole").change(function () {
			var roleId = $(this).val();

			$.ajax({
				type: 'GET',
				dataType: 'json',
				url: protocol + '//' + host + '/api/role/get-salary-range-for-role/' + roleId,
				success: function (data) {
					var range = JSON.parse(JSON.stringify(data));

					tippy('#Salary', {
						allowHTML: true,
						content: 'Минимална заплата: ' + range.minimumSalary + ' лв.<br/>' + 'Максимална заплата: ' + range.maximumSalary + ' лв.'
					});
                }
            })
		});
    }

	if (pathName.includes("edit-employee")) {
		var departmentId = $("#selectDepartment").val();
		var currentEmployeeRole = null;
		var currentEmployeeEmail = $("#inputEmail").val();
		$.ajax({
			type: 'GET',
			dataType: 'json',
			url: protocol + '//' + host + '/api/role/get-current-employee-role/' + currentEmployeeEmail,
			success: function (data) {
				var role = JSON.parse(JSON.stringify(data));
				currentEmployeeRole = role.RoleName;

				$.ajax({
					type: 'GET',
					dataType: 'json',
					url: protocol + '//' + host + '/api/department/get-roles-for-department/' + departmentId,
					success: function (data) {
						$("#selectRole").find('option:not(:first)').remove();
						$("#selectRole").prop("readonly", false);

						var roles = JSON.parse(JSON.stringify(data));
						for (var item in roles) {
							if (roles[item].RoleName == currentEmployeeRole) {
								$("#selectRole").append('<option value="' + roles[item].Id + '" selected>' + roles[item].RoleName + "</option>");
							} else {
								$("#selectRole").append('<option value="' + roles[item].Id + '">' + roles[item].RoleName + "</option>");
							}
						}
					}
				});
			}
		});

		
	}

	$("#selectDepartment").change(function() {
		var departmentId = $(this).val();
		$.ajax({ 
			type: 'GET',
			dataType: 'json',
			url: protocol + '//' + host + '/api/department/get-roles-for-department/' + departmentId,
			success: function(data) 
			{
				$("#selectRole").find('option:not(:first)').remove();
				$("#selectRole").prop("readonly", false);
					var roles = JSON.parse(JSON.stringify(data));
					for (var item in roles) {
						$("#selectRole").append('<option value="' + roles[item].Id + '">' + roles[item].RoleName + "</option>");
					}
				
			}
		});
	});

	var d = new Date();
	var strDate = d.getDate() + "." + (d.getMonth() + 1) + "." + d.getFullYear();

	$('.input-daterange').datepicker({
		format: "dd-mm-yyyy",
		weekStart: 1,
		language: "bg",
		startDate: strDate,
		orientation: "top auto",
		daysOfWeekDisabled: "0,6",
		todayHighlight: true
	});

	$('.input-daterange input').each(function () {
		$(this).datepicker('clearDates');
	});

	if (pathName.includes("request-absence")) {
		$.ajax({
			type: 'GET',
			dataType: 'json',
			url: protocol + '//' + host + '/api/absence/get-absence-reasons/',
			success: function (data) {
				//$("#selectReason").prop("readonly", false);
				var reasons = JSON.parse(JSON.stringify(data));
				for (var item in reasons) {
					//$("#reasons").append('<option value="' + reasons[item].ReasonName + '">');
					$("#selectReason").append('<option value="' + reasons[item].ReasonName + '">' + reasons[item].ReasonName + "</option>");
				}
				
			}
		});

		$("#startDateAbsence").change(function () {
			var date = $(this).val().toString("yyyy-MM-dd");
			var startDate = new Date(date);
			var endDate = new Date(date);

			if ($("#selectReason")[0].selectedIndex === 2 ||
				$("#selectReason")[0].selectedIndex === 3 ||
				$("#selectReason")[0].selectedIndex === 6) {

				if (endDate.getDay() == 5) {
					endDate.setDate(startDate.getDate() + 3);
				} else {
					endDate.setDate(startDate.getDate() + 1);
				}
				$('#endDateAbsence').prop("value", endDate.toISOString().substring(0, 10));
				$('#endDateAbsence').prop("max", endDate.toISOString().substring(0, 10));
			} else {
				$('#endDateAbsence').removeAttr("max");
            }
		});
	}

	if (pathName.includes("calendar-absences")) {
		var currentYear = new Date().getFullYear();
		var startDate = new Date(currentYear, 0, 1);
		var endDate = new Date(currentYear, 11, 31);
		
		var calendarEl = document.getElementById('calendar');
		var calendar = new FullCalendar.Calendar(calendarEl, {
			initialView: (function () { if ($(window).width() <= 768) { return initialView = 'dayGridWeek'; } else { return initialView = 'dayGridMonth'; } })(),
			fixedWeekCount: false,
			locale: 'bg',
			themeSystem: 'standard',
			dayMaxEventRows: true, // for all non-TimeGrid views
			views: {
				timeGrid: {
					dayMaxEventRows: 5 // adjust to 6 only for timeGridWeek/timeGridDay
				}
			},
			validRange: {
				start: startDate,
				end: endDate
			},
			eventDidMount: function (info) {
				if (info?.extendedProps?.description != 'undefined')  {
					if (info?.event?.extendedProps.period) {
						tippy(info.el, {
							content: info.event.extendedProps.description + "<br/>" +
								info.event.extendedProps.period,
							allowHTML: true,
						});
					} else {
						tippy(info.el, {
							content: info.event.extendedProps.description,
							allowHTML: true,
						});
                    }
				}
			},
			eventSources: [
				{
					url: '/api/absence/get-approved-paid-absences-for-current-year/',
					color: '#38c3d9'
				},
				{
					url: '/api/holiday/get-holidays-for-current-year',
					color: '#32a852'
				},
				{
					url: '/api/absence/get-approved-unpaid-absences-for-current-year/',
					color: '#ff6961'
				},
				{
					url: '/api/absence/get-approved-sick-absences-for-current-year/',
					color: '#e1ad01'
				},
				{
					url: '/api/absence/get-approved-other-absences-for-current-year/',
					color: '#6f2da8'
				},
			]
				 
			
		});
		calendar.render();

		var sources = calendar.getEventSources();

	}

	if (pathName.includes("approve-salaries")) {
		$(".bonusCheckbox").click(function () {
			if ($(this).prop('checked')) {
				$(this).parent().find('.form-floating').show(1000);
			} else {
				$(this).parent().find('.form-floating').hide(1000);
			}
		});
	}

	if (pathName.includes("view-salary-history")) {
		$("[data-bs-toggle=tooltip").tooltip();

		$('#salaryModal').on('show.bs.modal', function (e) {

			var modal = $(this);

			var grossSalary = $(e.relatedTarget).data("salary-gross");

			var paidLeaveUsed = parseInt($(e.relatedTarget).data("paid-leave-days"));
			var paidLeaveCost = 0;
			if (paidLeaveUsed) {

				$(".leave").show();
				$("#paidLeaveRow").show();
				if (paidLeaveUsed == 1) {
					$("#paidLeaveDays").text("1 ден");
				} else if (paidLeaveUsed > 1) {
					$("#paidLeaveDays").text(paidLeaveUsed + " дни");
				}

				paidLeaveCost = parseFloat(new String($(e.relatedTarget).data("paid-leave-cost")).replace(',','.'));

				$("#paidLeaveCost").text(paidLeaveCost + " лв.");
			}

			var unpaidLeaveUsed = parseInt($(e.relatedTarget).data("unpaid-leave-days"));

			if (unpaidLeaveUsed) {
				$(".leave").show();
				$("#unpaidLeaveRow").show();

				if (unpaidLeaveUsed == 1) {
					$("#unpaidLeaveDays").text("1 ден");
				} else if (unpaidLeaveUsed > 1) {
					$("#unpaidLeaveDays").text(unpaidLeaveUsed + " дни");
				}

				$("#unpaidLeaveCost").text("0.00 лв.");
			}

			var sickLeaveUsed = parseInt($(e.relatedTarget).data("sick-leave-days"));
			var sickLeaveCost = 0;
			if (sickLeaveUsed) {
				$(".leave").show();
				$("#sickLeaveRow").show();

				if (sickLeaveUsed == 1) {
					$("#sickLeaveDays").text("1 ден");
				} else if (sickLeaveUsed > 1) {
					$("#sickLeaveDays").text(sickLeaveUsed + " дни");
				}

				sickLeaveCost = parseFloat(new String($(e.relatedTarget).data("sick-leave-cost")).replace(',', '.'));
				$("#sickLeaveCost").text(sickLeaveCost + " лв.");
			}

			var otherLeaveUsed = parseInt($(e.relatedTarget).data("other-leave-days"));
			var otherLeaveCost = 0;
			if (otherLeaveUsed) {
				$(".leave").show();
				$("#otherLeaveRow").show();

				if (otherLeaveUsed == 1) {
					$("#otherLeaveDays").text("1 ден");
				} else if (otherLeaveUsed > 1) {
					$("#otherLeaveDays").text(otherLeaveUsed + " дни");
				}

				otherLeaveCost = parseFloat(new String($(e.relatedTarget).data("other-leave-cost")).replace(',', '.'));
				$("#otherLeaveCost").text(otherLeaveCost + " лв.");
			}

			if (Number.isNaN(paidLeaveUsed)) {
				$("#paidLeaveRow").hide();
			}


			if (Number.isNaN(unpaidLeaveUsed)) {
				$("#unpaidLeaveRow").hide();
			}

			if (Number.isNaN(sickLeaveUsed)) {
				$("#sickLeaveRow").hide();
			}

			if (Number.isNaN(otherLeaveUsed)) {
				$("#otherLeaveRow").hide();
			}

			if (Number.isNaN(paidLeaveUsed) &&
				Number.isNaN(unpaidLeaveUsed) &&
				Number.isNaN(sickLeaveUsed) &&
				Number.isNaN(otherLeaveUsed)) {
				$(".leave").hide();
				$("#paidLeaveRow").hide();
				$("#unpaidLeaveRow").hide();
				$("#sickLeaveRow").hide();
				$("#otherLeaveRow").hide();
			}

			modal.find("#grossSalary").text(grossSalary.toFixed(2) + " лв.");

			var month = $(e.relatedTarget).data("month");
			modal.find("#month").text(month);

			var year = $(e.relatedTarget).data("year");
			modal.find("#year").text(year);

			var workedOutDays = parseInt($(e.relatedTarget).data("worked-out-days"));
			var workedOutSalary = parseFloat(new String($(e.relatedTarget).data("worked-out-salary")).replace(',','.'));
			var workDaysInMonth = parseInt($(e.relatedTarget).data("workdays-in-month"));

			var oneBusinesDayCost = grossSalary / workDaysInMonth;

			$('#workDaysInMonth').text(workDaysInMonth + " дни");
			$('#workedOutDays').text(workedOutDays + " дни");

			var grossAmount = parseFloat((workedOutSalary + (paidLeaveCost + otherLeaveCost)));


			var valueDOO = 0;
			var valueDZPO = 0;
			var valueZO = 0;
			var socialSecurityIncome = 0;

			if (!Number.isNaN(sickLeaveUsed)) {
				grossAmount = parseFloat(grossAmount) + parseFloat(sickLeaveCost);
			}

			valueDOO = parseFloat((grossAmount * (8.38 / 100)).toFixed(2));
			valueDZPO = parseFloat((grossAmount * (2.20 / 100)).toFixed(2));
			valueZO = parseFloat((grossAmount * (3.20 / 100)).toFixed(2));


			if (grossAmount < 3000) {
				valueDOO = parseFloat((grossAmount * (8.38 / 100)).toFixed(2));
				valueDZPO = parseFloat((grossAmount * (2.20 / 100)).toFixed(2));
				valueZO = parseFloat((grossAmount * (3.20 / 100)).toFixed(2));

				socialSecurityIncome = grossAmount;
			} else {
				valueDOO = parseFloat((3000 * (8.38 / 100)).toFixed(2));
				valueDZPO = parseFloat((3000 * (2.20 / 100)).toFixed(2));
				valueZO = parseFloat((3000 * (3.20 / 100)).toFixed(2));

				socialSecurityIncome = 3000;
			}
            

			modal.find("#SSI").text(parseFloat(socialSecurityIncome).toFixed(2) + " лв.");
			modal.find("#valueDOO").text(valueDOO.toFixed(2) + " лв.");
			modal.find("#valueDZPO").text(valueDZPO.toFixed(2) + " лв.");
			modal.find("#valueZO").text(valueZO.toFixed(2) + " лв.");

			var socialSecurityContributionsByEmployee = parseFloat(Math.round(((valueDOO + valueDZPO + valueZO) + Number.EPSILON) * 100) / 100);
			modal.find("#employeeSSC").text(socialSecurityContributionsByEmployee.toFixed(2) + " лв.");
			var taxBase = 0;
			if (Number.isNaN(sickLeaveUsed)) {
				taxBase = grossAmount - socialSecurityContributionsByEmployee;
			} else {
				taxBase = (grossAmount - sickLeaveCost) - socialSecurityContributionsByEmployee;
            }


			var incomeTax = Math.round(((taxBase * 0.1) + Number.EPSILON) * 100) / 100;

			modal.find("#incomeTax").text(incomeTax.toFixed(2) + " лв.");

			var totalEmployeeContributions = parseFloat((socialSecurityContributionsByEmployee + incomeTax).toFixed(2));
			modal.find("#tec").text(totalEmployeeContributions.toFixed(2) + " лв.");

			var netSalaryToReceive = grossAmount - (socialSecurityContributionsByEmployee + incomeTax);

			var bonus = $(e.relatedTarget).data("bonus");

			if (bonus) {
				$("#netSalarybonusRow").show();
				$("#netSalaryBeforeBonus").html('<b>' + netSalaryToReceive.toFixed(2) + ' лв.</b>');

				$("#bonusRow").show();
				$("#bonus").text(bonus + " лв.");

				modal.find("#netSalaryToReceive").html("<b>" + (netSalaryToReceive + bonus).toFixed(2) + " лв.</b>");
			} else {
				$("#netSalarybonusRow").hide();
				$("#bonusRow").hide();

				modal.find("#netSalaryToReceive").html("<b>" + netSalaryToReceive.toFixed(2) + " лв.</b>");
			}
		});
	}

	$("#startDateAbsence").change(function () {
		$("#endDateAbsence").attr("min", $(this).val());
		if ($(this).val() > $("#endDateAbsence").val()) {
			$("#endDateAbsence").val($(this).val());
		}
	});
});