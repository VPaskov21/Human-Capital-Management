$(document).ready(function () {
	var host = $(location).attr('host');
	var protocol = $(location).attr('protocol');
	var pathName = $(location).attr('pathname');
	var search = $(location).attr('search');

	/*$("form").submit(function (e) {
		alert('submit intercepted');
		e.preventDefault();
		console.log($('form').serializeArray());
	});*/

	var timeoutId = 0;

	setTimeout(function () {
		$(".alert").hide(1000);
	}, 4000); // <-- time in milliseconds

	setTimeout(function () {
		$(".alert").addClass("d-none");
	}, 5000); // <-- time in milliseconds

	//Department validation
	$('#DepartmentName').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateDepartmentName, 300);
	});

	function validateDepartmentName() {
		var departmentName = $('#DepartmentName').val();
		if (/^([a-zA-Zа-яА-Я]+)*([ ]{1}[a-zA-Zа-яА-Я]+)*$/.test(departmentName) == true && departmentName.length > 0) {
			if ($('#DepartmentName').hasClass("is-invalid")) {
				$('#DepartmentName').removeClass("is-invalid");
				$('#DepartmentName').parent().remove('.invalid-feedback');
			}
			$('#DepartmentName').addClass("is-valid");
			return true;
		} else {
			$('#DepartmentName').removeClass("is-valid");
			$('#DepartmentName').removeClass("is-invalid");
			$('#DepartmentName').addClass("is-invalid");
			$('#DepartmentName').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (departmentName.length == 0) {
				$('#DepartmentName').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			} else {
				$('#DepartmentName').parent().append("<div class='invalid-feedback'>Името трябва да съдържа само букви или интервал.</div>");
			}
			return false;
		}
	}
	//End of department validation

	//Role validation
	$('#RoleName').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateRoleName, 300);
	});

	function validateRoleName() {
		var roleName = $('#RoleName').val();
		if (/^([a-zA-Zа-яА-Я]+)*([,]?[ ]{1}[a-zA-Zа-яА-Я]+)*$/.test(roleName) == true && roleName.length > 0) {
			if ($('#RoleName').hasClass("is-invalid")) {
				$('#RoleName').removeClass("is-invalid");
				$('#RoleName').parent().remove('.invalid-feedback');
			}
			$('#RoleName').addClass("is-valid");
			return true;
		} else {
			$('#RoleName').removeClass("is-valid");
			$('#RoleName').removeClass("is-invalid");
			$('#RoleName').addClass("is-invalid");
			$('#RoleName').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (roleName.length == 0) {
				$('#RoleName').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			} else {
				$('#RoleName').parent().append("<div class='invalid-feedback'>Името трябва да съдържа само букви, интервал и запетая.</div>");
			}
			return false;
		}
	}
	//End of role validation

	//Min Salary validation
	$('#MinSalary').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateMinSalary, 300);
	});

	function validateMinSalary() {
		var salary = $('#MinSalary').val();
		if ((/^[0-9]+[.]{1}[0-9]{2}$/.test(salary) == true) && salary.length >= 1) {
			if ($('#MinSalary').hasClass("is-invalid")) {
				$('#MinSalary').removeClass("is-invalid");
				$('#MinSalary').parent().remove('.invalid-feedback');
			}
			$('#MinSalary').addClass("is-valid");
			return true;
		} else {
			$('#MinSalary').removeClass("is-valid");
			$('#MinSalary').removeClass("is-invalid");
			$('#MinSalary').addClass("is-invalid");
			$('#MinSalary').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (salary.length == 0) {
				$('#MinSalary').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			}
			else {
				$('#MinSalary').parent().append("<div class='invalid-feedback'>Сумата за заплата може да съдъра само цифри и точка и да бъде с точност 2 символа.</div>");
			}
			return false;
		}
	}
	//End of min salary validation

	//Max Salary validation
	$('#MaxSalary').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateMaxSalary, 300);
	});

	function validateMaxSalary() {
		var salary = $('#MaxSalary').val();
		if ((/^[0-9]+[.]{1}[0-9]{2}$/.test(salary) == true) && salary.length >= 1) {
			if ($('#MaxSalary').hasClass("is-invalid")) {
				$('#MaxSalary').removeClass("is-invalid");
				$('#MaxSalary').parent().remove('.invalid-feedback');
			}
			$('#MaxSalary').addClass("is-valid");
			return true;
		} else {
			$('#MaxSalary').removeClass("is-valid");
			$('#MaxSalary').removeClass("is-invalid");
			$('#MaxSalary').addClass("is-invalid");
			$('#MaxSalary').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (salary.length == 0) {
				$('#MaxSalary').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			}
			else {
				$('#MaxSalary').parent().append("<div class='invalid-feedback'>Сумата за заплата може да съдъра само цифри и точка и да бъде с точност 2 символа.</div>");
			}
			return false;
		}
	}
	//End of max salary validation

	function validateRoleForm() {
		if (!validateRoleName()) {
			document.getElementById("RoleName").scrollIntoView();
			return false;
		}

		if (!validateMinSalary()) {
			document.getElementById("MinSalary").scrollIntoView();
			return false;
		}

		if (!validateMaxSalary()) {
			document.getElementById("MaxSalary").scrollIntoView();
			return false;
		}

		if ($("#selectDepartment").val() === "Избери отдел") {
			document.getElementById("selectDepartment").scrollIntoView();
			return false;
		}

		return true;
    }

	$("#createDepartmentForm").submit(function (e) {
		if (validateDepartmentName()) {
			console.log("Fields validated successfully.");
		} else {
			console.log("Error on field.");
			e.preventDefault();
        }
	});

	$("#createRoleForm").submit(function (e) {
		if (validateRoleForm()) {
			console.log("Fields validated successfully.");
		} else {
			console.log("Error on field.");
			e.preventDefault();
		}
	});
	

	$(document).on("submit", "form", function (e) {
		console.log($('form').serializeArray());
	});

	$("#checkboxHR").change(function () {
		if (this.checked) {
			$(this).val(true);
		}
		else {
			$(this).val(false);
		}
	});

	var i = $("tbody").children().length;
	$("#add_row").click(function () {
		var content = '<th class="w-50 align-middle"><input type="text" class="form-control" id="z' + i + '__HolidayName" name="[' + i + '].HolidayName"></th>';
		content += '<td class="w-25 align-middle"><input type="date" class="form-control" id="holidayDate" data-val="true" data-val-required="The HolidayDate field is required." name="[' + i + '].HolidayDate"></td>';
		content += '<td class="w-25 align-middle"><input type="date" class="form-control" id="actualAbsenceDay" name="[' + i + '].ActualAbsenceDay"></td>';

		var $row = $('<tr>' + content + '</tr>');

		$('tbody').append($row);
		$('tr').last().show('slow');
		$row.hide().show('slow');
		$("#delete_row").prop('hidden', false);
		i++;
	});

	$("#delete_row").click(function () {
		if (i > 0) {
			$("tr").last().hide('slow', function () { $(this).remove(); });
		}

		i--;
		if (i == 0) {
			$("#delete_row").prop('hidden', true);
		}
	});

	tippy('#actualAbsenceDayCol', {
		allowHTML: true,
		content: 'Ако датата на празника е в Събота или Неделя'
	});

	console.log($("tbody").children().length);
});