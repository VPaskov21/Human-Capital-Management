$(document).ready(function () {
	var host = $(location).attr('host');
	var protocol = $(location).attr('protocol');
	var pathName = $(location).attr('pathname');

	console.log(pathName);

	var timeoutId = 0;

	//First name validation
	$('#FirstName').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateFirstName, 200);
	});

	function validateFirstName() {
		var firstName = $('#FirstName').val();
		if ((/^([a-zA-Zа-яА-Я])+$/.test(firstName) == true) && firstName.length >= 1) {
			if ($('#FirstName').hasClass("is-invalid")) {
				$('#FirstName').removeClass("is-invalid");
				$('#FirstName').parent().remove('.invalid-feedback');
			}
			$('#FirstName').addClass("is-valid");
			return true;
		}
		else {
			$('#FirstName').removeClass("is-valid");
			$('#FirstName').removeClass("is-invalid");
			$('#FirstName').addClass("is-invalid");
			$('#FirstName').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (firstName.length == 0) {
				$('#FirstName').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			} else {
				$('#FirstName').parent().append("<div class='invalid-feedback'>Името трябва да съдържа само букви.</div>");
			}
			return false;
		}
	}
	//End of first name validation

	//Middle name validation
	$('#MiddleName').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateMiddleName, 200);
	});

	function validateMiddleName() {
		var firstName = $('#MiddleName').val();
		if ((/^([a-zA-Zа-яА-Я])+$/.test(firstName) == true) && firstName.length >= 1) {
			if ($('#MiddleName').hasClass("is-invalid")) {
				$('#MiddleName').removeClass("is-invalid");
				$('#MiddleName').parent().remove('.invalid-feedback');
			}
			$('#MiddleName').addClass("is-valid");
			return true;
		}
		else {
			$('#MiddleName').removeClass("is-valid");
			$('#MiddleName').removeClass("is-invalid");
			$('#MiddleName').addClass("is-invalid");
			$('#MiddleName').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (firstName.length == 0) {
				$('#MiddleName').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			} else {
				$('#MiddleName').parent().append("<div class='invalid-feedback'>Името трябва да съдържа само букви.</div>");
			}
			return false;
		}
	}
	//End of middle name validation

	//Last name validation
	$('#LastName').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateLastName, 200);
	});

	function validateLastName() {
		var firstName = $('#LastName').val();
		if ((/^([a-zA-Zа-яА-Я])+$/.test(firstName) == true) && firstName.length >= 1) {
			if ($('#LastName').hasClass("is-invalid")) {
				$('#LastName').removeClass("is-invalid");
				$('#LastName').parent().remove('.invalid-feedback');
			}
			$('#LastName').addClass("is-valid");
			return true;
		}
		else {
			$('#LastName').removeClass("is-valid");
			$('#LastName').removeClass("is-invalid");
			$('#LastName').addClass("is-invalid");
			$('#LastName').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (firstName.length == 0) {
				$('#LastName').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			} else {
				$('#LastName').parent().append("<div class='invalid-feedback'>Името трябва да съдържа само букви.</div>");
			}
			return false;
		}
	}
	//End of last name validation

	//Email validation
	$('#Email').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateEmailAddress, 300);
	});

	function validateEmailAddress() {
		var emailAddress = $('#Email').val();
		if ((/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(emailAddress) == true) && emailAddress.length >= 1) {
			if ($('#Email').hasClass("is-invalid")) {
				$('#Email').removeClass("is-invalid");
				$('#Email').parent().remove('.invalid-feedback');
			}
			$('#Email').addClass("is-valid");
			return true;
		} else {
			$('#Email').removeClass("is-valid");
			$('#Email').removeClass("is-invalid");
			$('#Email').addClass("is-invalid");
			$('#Email').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (emailAddress.length == 0) {
				$('#Email').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			}
			else {
				$('#Email').parent().append("<div class='invalid-feedback'>Невалиден email адрес.</div>");
			}
			return false;
		}
	}
	//End of email validation

	//Phone validation
	$('#PhoneNumber').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validatePhoneNumber, 300);
	});

	function validatePhoneNumber() {
		var phoneNumber = $('#PhoneNumber').val();
		if ((/^(0)(87|88|89|98)[0-9]{7}$/.test(phoneNumber) == true) && phoneNumber.length >= 1) {
			if ($('#PhoneNumber').hasClass("is-invalid")) {
				$('#PhoneNumber').removeClass("is-invalid");
				$('#PhoneNumber').parent().remove('.invalid-feedback');
			}
			$('#PhoneNumber').addClass("is-valid");
			return true;
		} else {
			$('#PhoneNumber').removeClass("is-valid");
			$('#PhoneNumber').removeClass("is-invalid");
			$('#PhoneNumber').addClass("is-invalid");
			$('#PhoneNumber').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (phoneNumber.length == 0) {
				$('#PhoneNumber').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			}
			else {
				$('#PhoneNumber').parent().append("<div class='invalid-feedback'>Невалиден телефонен номер.</div>");
			}
			return false;
		}
	}
	//End of phone validation

	//Region validation
	$('#selectRegion').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateRegion, 300);
	});

	function validateRegion() {
		var region = $('#selectRegion').val();
		if ((/^[a-zA-Zа-яА-Я]*$/.test(region) == true) && region.length >= 1) {
			if ($('#selectRegion').hasClass("is-invalid")) {
				$('#selectRegion').removeClass("is-invalid");
				$('#selectRegion').parent().remove('.invalid-feedback');
			}
			$('#selectRegion').addClass("is-valid");
			return true;
		} else {
			$('#selectRegion').removeClass("is-valid");
			$('#selectRegion').removeClass("is-invalid");
			$('#selectRegion').addClass("is-invalid");
			$('#selectRegion').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (region.length == 0) {
				$('#selectRegion').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			}
			else {
				$('#selectRegion').parent().append("<div class='invalid-feedback'>Името на областта трябва да съдържа само букви.</div>");
			}
			return false;
		}
	}
	//End of region validation

	//City validation
	$('#selectCity').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateCity, 300);
	});

	function validateCity() {
		var city = $('#selectCity').val();
		if ((/^[a-zA-Zа-яА-Я]*$/.test(city) == true) && city.length >= 1) {
			if ($('#selectCity').hasClass("is-invalid")) {
				$('#selectCity').removeClass("is-invalid");
				$('#selectCity').parent().remove('.invalid-feedback');
			}
			$('#selectCity').addClass("is-valid");
			return true;
		} else {
			$('#selectCity').removeClass("is-valid");
			$('#selectCity').removeClass("is-invalid");
			$('#selectCity').addClass("is-invalid");
			$('#selectCity').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (city.length == 0) {
				$('#selectCity').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			} else {
				$('#selectCity').parent().append("<div class='invalid-feedback'>Името на градът трябва да съдържа само букви.</div>");
			}
			return false;
		}
	}
	//End of city validation

	//Postal Code validation
	$('#inputPostalCode').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validatePostalCode, 300);
	});

	function validatePostalCode() {
		var postalCode = $('#inputPostalCode').val();
		if ((/^[1-9]{1}[0-9]{3}$/.test(postalCode) == true) && postalCode.length == 4) {
			if ($('#inputPostalCode').hasClass("is-invalid")) {
				$('#inputPostalCode').removeClass("is-invalid");
				$('#inputPostalCode').parent().remove('.invalid-feedback');
			}
			$('#inputPostalCode').addClass("is-valid");
			return true;
		} else {
			$('#inputPostalCode').removeClass("is-valid");
			$('#inputPostalCode').removeClass("is-invalid");
			$('#inputPostalCode').addClass("is-invalid");
			$('#inputPostalCode').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (/^[a-zA-Z0-9а-яА-Я- ,.]*$/.test(postalCode)) {
				$('#inputPostalCode').parent().append("<div class='invalid-feedback'>Пощенският код трябва да съдържа само цифри.</div>");
			}
			if (postalCode.length != 4) {
				$('#inputPostalCode').parent().append("<div class='invalid-feedback'>Пощенският код трябва да бъде 4 цифрен.</div>");
			}
			return false;
		}
	}
	//End of postal code validation

	//Address validation
	$('#Address').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateAddress, 300);
	});

	function validateAddress() {
		var address = $('#Address').val();
		if ((/^[a-zA-Z0-9а-яА-Я- ,.]*$/.test(address) == true) && address.length >= 1) {
			if ($('#Address').hasClass("is-invalid")) {
				$('#Address').removeClass("is-invalid");
				$('#Address').parent().remove('.invalid-feedback');
			}
			$('#Address').addClass("is-valid");
			return true;
		} else {
			$('#Address').removeClass("is-valid");
			$('#Address').removeClass("is-invalid");
			$('#Address').addClass("is-invalid");
			$('#Address').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (address.length == 0) {
				$('#Address').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			}
			else {
				$('#Address').parent().append("<div class='invalid-feedback'>Адресът трябва да съдържа само букви, цифри, точка или запетая.</div>");
			}
			return false;
		}
	}
	//End of address validation

	//Additional address validation
	$('#Additional_Address').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateAdditionalAddress, 300);
	});

	function validateAdditionalAddress() {
		var additional_address = $('#Additional_Address').val();
		if (/^[a-zA-Z0-9а-яА-Я- ,.]*$/.test(additional_address) == true) {
			if ($('#Additional_Address').hasClass("is-invalid")) {
				$('#Additional_Address').removeClass("is-invalid");
				$('#Additional_Address').parent().remove('.invalid-feedback');
			}
			$('#Additional_Address').addClass("is-valid");
			return true;
		} else {
			$('#Additional_Address').removeClass("is-valid");
			$('#Additional_Address').removeClass("is-invalid");
			$('#Additional_Address').addClass("is-invalid");
			$('#Additional_Address').parent().find('.invalid-feedback').remove('.invalid-feedback');
			$('#Additional_Address').parent().append("<div class='invalid-feedback'>Адресът трябва да съдържа само букви, цифри, точка или запетая.</div>");
			return false;
		}
	}
	//End of additional address validation

	//Salary validation
	$('#Salary').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateSalary, 300);
	});

	function validateSalary() {
		var salary = $('#Salary').val();
		if ((/^[0-9]+([.]{1}[0-9]{2})?$/.test(salary) == true) && salary.length >= 1) {
			if ($('#Salary').hasClass("is-invalid")) {
				$('#Salary').removeClass("is-invalid");
				$('#Salary').parent().remove('.invalid-feedback');
			}
			$('#Salary').addClass("is-valid");
			return true;
		} else {
			$('#Salary').removeClass("is-valid");
			$('#Salary').removeClass("is-invalid");
			$('#Salary').addClass("is-invalid");
			$('#Salary').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (salary.length == 0) {
				$('#Salary').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			}
			else {
				$('#Salary').parent().append("<div class='invalid-feedback'>Сумата за заплата може да съдъра само цифри и точка и да бъде с точност 2 символа.</div>");
			}
			return false;
		}
	}
	//End of salary validation

	//Old password validation
	$('#oldPassword').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateOldPassword, 300);
	});

	function validateOldPassword() {
		var password = $('#oldPassword').val();
		if (password.length > 0) {
			if ($('#oldPassword').hasClass("is-invalid")) {
				$('#oldPassword').removeClass("is-invalid");
				$('#oldPassword').parent().remove('.invalid-feedback');
			}
			$('#oldPassword').addClass("is-valid");
			return true;
		} else {

			$('#oldPassword').removeClass("is-valid");
			$('#oldPassword').removeClass("is-invalid");
			$('#oldPassword').addClass("is-invalid");
			$('#oldPassword').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (password.length == 0) {
				$('#oldPassword').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			}
			return false;
		}
	}
	//End of old password validation

	//Password validation
	$('#newPassword').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateNewPassword, 300);
	});

	function validateNewPassword() {
		var password = $('#newPassword').val();
		if (password.length > 8) {
			if ($('#newPassword').hasClass("is-invalid")) {
				$('#newPassword').removeClass("is-invalid");
				$('#newPassword').parent().remove('.invalid-feedback');
			}
			$('#newPassword').addClass("is-valid");
			return true;
		} else {

			$('#newPassword').removeClass("is-valid");
			$('#newPassword').removeClass("is-invalid");
			$('#newPassword').addClass("is-invalid");
			$('#newPassword').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (password.length == 0) {
				$('#newPassword').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			} else {
				$('#newPassword').parent().append("<div class='invalid-feedback'>Паролата трябва да бъде повече от 8 символа.</div>");
			}

			return false;
		}
	}
	//End of password validation

	//Confirm Password validation
	$('#confirmPassword').keydown(function () {
		clearTimeout(timeoutId);
		timeoutId = setTimeout(validateConfirmPassword, 300);
	});

	function validateConfirmPassword() {
		var newPassword = $("#newPassword").val();
		var password = $('#confirmPassword').val();
		if (password.length > 8 && password === newPassword) {
			if ($('#confirmPassword').hasClass("is-invalid")) {
				$('#confirmPassword').removeClass("is-invalid");
				$('#confirmPassword').parent().remove('.invalid-feedback');
			}
			$('#confirmPassword').addClass("is-valid");
			return true;
		} else {

			$('#confirmPassword').removeClass("is-valid");
			$('#confirmPassword').removeClass("is-invalid");
			$('#confirmPassword').addClass("is-invalid");
			$('#confirmPassword').parent().find('.invalid-feedback').remove('.invalid-feedback');
			if (password.length == 0) {
				$('#confirmPassword').parent().append("<div class='invalid-feedback'>Полето не може да бъде празно.</div>");
			}

			else if (newPassword != password) {
				$('#confirmPassword').parent().append("<div class='invalid-feedback'>Паролите трябва да съвпадат.</div>");
			}

			else if (password.length < 8) {
				$('#confirmPassword').parent().append("<div class='invalid-feedback'>Паролата трябва да бъде повече от 8 символа.</div>");
			}

			return false;
		}
	}
	//End of confirm password validation


	var maxDate = new Date(new Date().getFullYear() - 18, new Date().getMonth(), new Date().getDate());

	$("#Birthday").attr("max", maxDate.toISOString().split('T')[0]);

	function validateStartDate() {
		if ($("#StartDate").val()) {
			return true;
		} 
		else {
			return false;
        }
	}

	function validateBirthday() {
		if ($("#Birthday").val()) {
			return true;
		}
		else {
			return false;
		}
	}

	function validateAllFields() {
		if (!validateFirstName()) {
			document.getElementById("FirstName").scrollIntoView();
			return false;
		}

		if (!validateMiddleName()) {
			document.getElementById("MiddleName").scrollIntoView();
			return false;
		}

		if (!validateLastName()) {
			document.getElementById("LastName").scrollIntoView();
			return false;
		}

		if (!validateEmailAddress()) {
			document.getElementById("Email").scrollIntoView();
			return false;
		}

		if (!validatePhoneNumber()) {
			document.getElementById("PhoneNumber").scrollIntoView();
			return false;
		}

		if (!validateRegion()) {
			document.getElementById("Region").scrollIntoView();
			return false;
		}

		if (!validateCity()) {
			document.getElementById("City").scrollIntoView();
			return false;
		}

		if (!validatePostalCode()) {
			document.getElementById("PostalCode").scrollIntoView();
			return false;
		}

		if (!validateAddress()) {
			document.getElementById("Address").scrollIntoView();
			return false;
		}

		if (!validateAdditionalAddress()) {
			document.getElementById("Additional_Address").scrollIntoView();
			return false;
		}

		if (!validateSalary()) {
			document.getElementById("Salary").scrollIntoView();
			return false;
		}

		if (!validateStartDate()) {
			document.getElementById("StartDate").scrollIntoView();
			return false;
		}

		if (!validateBirthday()) {
			document.getElementById("Birthday").scrollIntoView();
			return false;
		}

		return true;
	}


	if (pathName.includes("add-employee")) {
		$("#addEmployeeForm").submit(function (e) {
			if (validateAllFields()) {
				console.log("Fields validated successfully.");
			} else {
				console.log("Error on field.");
				e.preventDefault();
			}
		});
	}

	if (pathName.includes("edit-employee")) {
		$("#editEmployeeForm").submit(function (e) {
			if (validateAllFields()) {
				console.log("Fields validated successfully.");
			} else {
				console.log("Error on field.");
				e.preventDefault();
			}
		});
    }

	if (pathName.includes("change-password")) {
		function validatePasswordFields() {
			if (!validateOldPassword()) {
				document.getElementById("oldPassword").scrollIntoView();
				return false;
			}

			if (!validateNewPassword()) {
				document.getElementById("newPassword").scrollIntoView();
				return false;
			}

			if (!validateConfirmPassword()) {
				document.getElementById("confirmPassword").scrollIntoView();
				return false;
			}

			return true;
		}

		function equalNewPasswords() {
			if ($("#newPassword").val() === $("#confirmPassword").val()) {
				return true;
			}

			return false;
		}

		$("#passwordForm").submit(function (e) {
			if (validatePasswordFields()) {
				if (equalNewPasswords()) {
					console.log("Fields validated successfully.");
				} else {
					e.preventDefault();
                }
			} else {
				console.log("Error on field.");
				e.preventDefault();
			}
		});
	}

	if (pathName.includes("change-address")) {
		function validateAddressFields() {
			if (!validateAddress()) {
				document.getElementById("Address").scrollIntoView();
				return false;
			}

			if (!validateAdditionalAddress()) {
				document.getElementById("Additional_Address").scrollIntoView();
				return false;
			}

			if (!validateRegion()) {
				document.getElementById("selectRegion").scrollIntoView();
				return false;
			}

			if (!validateCity()) {
				document.getElementById("selectCity").scrollIntoView();
				return false;
			}

			if (!validatePostalCode()) {
				document.getElementById("Additional_Address").scrollIntoView();
				return false;
			}

			return true;
		}

		

		$("#changeAddressForm").submit(function (e) {
			
			if (validateAddressFields()) {
				console.log("Fields validated successfully.");
			} else {
				console.log("Error on field.");
				e.preventDefault();
			}
		});
	}
});