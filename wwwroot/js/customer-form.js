$(document).ready(function() {
    setupFormValidation();
});

function setupFormValidation() {
    $('#customerForm').on('submit', function(e) {
        e.preventDefault();

        if (validateForm()) {
            if (window.location.pathname.includes('/Create')) {
                createCustomer();
            } else {
                updateCustomer();
            }
        }
    });
}

function validateForm() {
    let isValid = true;
    $('.is-invalid').removeClass('is-invalid');

    const requiredFields = ['firstName', 'lastName', 'email'];

    requiredFields.forEach(field => {
        const element = $('#' + field);
        const value = element.val().trim();

        if (!value) {
            showFieldError(element, field.charAt(0).toUpperCase() + field.slice(1) + ' is required');
            isValid = false;
        }
    });

    // Validate email format
    const email = $('#email').val().trim();
    if (email) {
        const atPos = email.indexOf('@');
        const lastDotPos = email.lastIndexOf('.');

        if (atPos < 0 || lastDotPos < 0 || atPos > lastDotPos) {
            showFieldError($('#email'), 'Please enter a valid email address');
            isValid = false;
        }
    }

    return isValid;
}

function showFieldError(element, message) {
    element.addClass('is-invalid');
    element.siblings('.invalid-feedback').text(message);
}

function createCustomer() {
    const customerData = {
        firstName: $('#firstName').val().trim(),
        lastName: $('#lastName').val().trim(),
        email: $('#email').val().trim(),
        phoneNumber: $('#phoneNumber').val().trim() || null,
        address: $('#address').val().trim() || null,
        city: $('#city').val().trim() || null,
        country: $('#country').val().trim() || null,
        isActive: $('#isActive').is(':checked')
    };

    $.ajax({
        url: '/api/customers',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(customerData),
        success: function(response) {
            if (response.success) {
                window.location.href = '/CustomersWeb';
            } else {
                alert('Error: ' + (response.message || 'Failed to create customer'));
            }
        },
        error: function(xhr) {
            console.error('Error creating customer:', xhr);
            if (xhr.status === 400 && xhr.responseJSON && xhr.responseJSON.errors) {
                Object.keys(xhr.responseJSON.errors).forEach(field => {
                    const element = $('#' + field.toLowerCase());
                    if (element.length) {
                        showFieldError(element, xhr.responseJSON.errors[field][0]);
                    }
                });
            } else {
                alert('Failed to create customer. Please try again.');
            }
        }
    });
}

function updateCustomer() {
    const customerId = $('#customerId').val();
    const customerData = {
        firstName: $('#firstName').val().trim(),
        lastName: $('#lastName').val().trim(),
        email: $('#email').val().trim(),
        phoneNumber: $('#phoneNumber').val().trim() || null,
        address: $('#address').val().trim() || null,
        city: $('#city').val().trim() || null,
        country: $('#country').val().trim() || null,
        isActive: $('#isActive').is(':checked')
    };

    $.ajax({
        url: '/api/customers/' + customerId,
        method: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(customerData),
        success: function(response) {
            if (response.success) {
                window.location.href = '/CustomersWeb';
            } else {
                alert('Error: ' + (response.message || 'Failed to update customer'));
            }
        },
        error: function(xhr) {
            console.error('Error updating customer:', xhr);
            if (xhr.status === 400 && xhr.responseJSON && xhr.responseJSON.errors) {
                Object.keys(xhr.responseJSON.errors).forEach(field => {
                    const element = $('#' + field.toLowerCase());
                    if (element.length) {
                        showFieldError(element, xhr.responseJSON.errors[field][0]);
                    }
                });
            } else {
                alert('Failed to update customer. Please try again.');
            }
        }
    });
}

// Edit-specific functions
let originalCustomerData = {};

function loadCustomer() {
    const customerId = $('#customerId').val();

    if (!customerId) {
        showError();
        return;
    }

    $('#loadingSpinner').removeClass('d-none');

    $.ajax({
        url: '/api/customers/' + customerId,
        method: 'GET',
        success: function(response) {
            if (response.success && response.data) {
                originalCustomerData = response.data;
                populateForm(response.data);
                setupFormHandlers();
                $('#loadingSpinner').addClass('d-none');
                $('#customerContent').removeClass('d-none');
            } else {
                showError();
            }
        },
        error: function(xhr) {
            console.error('Error loading customer:', xhr);
            showError();
        }
    });
}

function showError() {
    $('#loadingSpinner').addClass('d-none');
    $('#errorState').removeClass('d-none');
}

function populateForm(customer) {
    $('#firstName').val(customer.firstName);
    $('#lastName').val(customer.lastName);
    $('#email').val(customer.email);
    $('#phoneNumber').val(customer.phoneNumber || '');
    $('#address').val(customer.address || '');
    $('#city').val(customer.city || '');
    $('#country').val(customer.country || '');
    $('#isActive').prop('checked', customer.isActive);

    if (customer.createdAt) {
        $('#createdAt').text(new Date(customer.createdAt).toLocaleString());
    }
    if (customer.updatedAt) {
        $('#updatedAt').text(new Date(customer.updatedAt).toLocaleString());
    }
}

function setupFormHandlers() {
    $('#resetBtn').on('click', function() {
        if (confirm('Are you sure you want to reset all changes?')) {
            populateForm(originalCustomerData);
            $('.is-invalid').removeClass('is-invalid');
        }
    });
}
