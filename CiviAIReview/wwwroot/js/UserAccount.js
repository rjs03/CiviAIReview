const _UserId = document.getElementById("userId");
const _MemberType = document.getElementById("memberType");
const _FirstName = document.getElementById("firstName");
const _MiddleName = document.getElementById("middleName");
const _LastName = document.getElementById("lastName");
const _Suffix = document.getElementById("suffix");
const _Address = document.getElementById("address");
const _EmailAddress = document.getElementById("emailAddress");
const _MobileNumber = document.getElementById("mobileNumber");
const _Status = document.getElementById("status");

const newButton = document.getElementById('btnNew');
const editButton = document.getElementById('btnEdit');
const cancelButton = document.getElementById('btnCancel');
const accessRightsButton = document.getElementById('btnAccessRights');
function ClearData() {
    document.getElementById('userId').value = null;
    document.getElementById('memberType').value = null;
    document.getElementById('firstName').value = null;
    document.getElementById('middleName').value = null;
    document.getElementById('lastName').value = null;
    document.getElementById('suffix').value = null;
    document.getElementById('address').value = null;
    document.getElementById('emailAddress').value = null;
    document.getElementById('mobileNumber').value = null;
    document.getElementById('status').value = null;
}

function ClearValues() {
    document.getElementById('btnNew').textContent = 'New';
    document.getElementById('btnEdit').textContent = 'Edit';
}

const form = document.querySelector("#userForm");

function DisableForm() {
    _UserId.disabled = true;
    _MemberType.disabled = true;
    _FirstName.disabled = true;
    _MiddleName.disabled = true;
    _LastName.disabled = true;
    _Suffix.disabled = true;
    _Address.disabled = true;
    _EmailAddress.disabled = true;
    _MobileNumber.disabled = true;
    _Status.disabled = true;
    editButton.disabled = true;
}
function EnableForm() {
    _UserId.disabled = false;
    _MemberType.disabled = false;
    _FirstName.disabled = false;
    _MiddleName.disabled = false;
    _LastName.disabled = false;
    _Suffix.disabled = false;
    _Address.disabled = false;
    _EmailAddress.disabled = false;
    _MobileNumber.disabled = false;
    _Status.disabled = false;
}

document.addEventListener("DOMContentLoaded", () => {
    DisableForm();
});


newButton.addEventListener("click", async function () {
    if (newButton.textContent === "New") {
        // Reset form to prepare for a new entry
        DisableForm();
        ClearData();
        EnableForm();
        _UserId.focus();
        newButton.textContent = "Save";
        return;
    }

    if (newButton.textContent === "Save") {
        // Collect form data
        const userData = {
            UserId: document.getElementById("userId").value,
            MemberType: document.getElementById("memberType").value,
            FirstName: document.getElementById("firstName").value,
            MiddleName: document.getElementById("middleName").value,
            LastName: document.getElementById("lastName").value,
            Suffix: document.getElementById("suffix").value,
            Address: document.getElementById("address").value,
            EmailAddress: document.getElementById("emailAddress").value,
            MobileNumber: document.getElementById("mobileNumber").value,
            Status: document.getElementById("status").value,
        };

        try {
            const response = await fetch("/User/SaveUserInformation", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(userData),
            });

            const data = await response.json();
            if (data.success) {
                alert(data.message);
                ClearData();
                DisableForm();
                loadUserData();
                newButton.textContent = "New";
            } else {
                alert(data.message);
            }
        } catch (error) {
            console.error("Error:", error);
            alert("An error occurred while saving.");
        }
    }
});

// Function to load user data from the server and populate the table
async function loadUserData() {
    const userTableBody = document.querySelector("#userTable tbody");

    try {
        const response = await fetch("/User/GetUsers");
        const users = await response.json();

        userTableBody.innerHTML = ""; // Clear old data

        if (users.length > 0) {
            users.forEach(user => {
                const row = `
                    <tr>
                        <td>${user.userID}</td>
                        <td>${user.memberType}</td>
                        <td>${user.firstName}</td>
                        <td>${user.middleName || ""}</td>
                        <td>${user.lastName}</td>
                        <td>${user.suffix || ""}</td>
                        <td>${user.address}</td>
                        <td>${user.emailAddress || ""}</td>
                        <td>${user.mobileNumber}</td>
                        <td>
                            ${user.status ? '<span class="badge bg-success">A</span>' : '<span class="badge bg-danger">I</span>'}
                        </td>
                    </tr>`;
                userTableBody.innerHTML += row;
            });
        } else {
            userTableBody.innerHTML = `<tr><td colspan="10" class="text-center">No records found.</td></tr>`;
        }

    } catch (error) {
        userTableBody.innerHTML = `<tr><td colspan="10" class="text-center text-danger">Error loading data.</td></tr>`;
    }
}

// Function to handle search filtering
function setupSearchFilter() {
    const searchInput = document.getElementById("searchBar");
    const userTableBody = document.querySelector("#userTable tbody");

    if (searchInput) {
        searchInput.addEventListener("input", () => {
            const searchValue = searchInput.value.toLowerCase();
            const tableRows = userTableBody.querySelectorAll("tr");

            tableRows.forEach(row => {
                const rowText = row.textContent.toLowerCase();
                row.style.display = rowText.includes(searchValue) ? "" : "none";
            });
        });
    } else {
        console.error("Search input not found!");
    }
}

// Initialize functions on page load
document.addEventListener("DOMContentLoaded", () => {
    loadUserData();
    setupSearchFilter();
    setupRowClickHandler();
    accessRightsButton.hidden = true;
});

cancelButton.addEventListener("click", function () {
    ClearData();
    DisableForm();
    newButton.textContent = "New";
    editButton.textContent = "Edit";
});
function setupRowClickHandler() {
    const userTableBody = document.querySelector("#userTable tbody");

    userTableBody.addEventListener("dblclick", (event) => {
        const clickedRow = event.target.closest("tr"); // Ensure we get the row, even if clicking on a cell

        if (clickedRow) {
            const cells = clickedRow.querySelectorAll("td");

            // Populate form fields (adjust IDs to match your form)
            document.getElementById("userId").value = cells[0].textContent.trim();
            document.getElementById("memberType").value = cells[1].textContent.trim();
            document.getElementById("firstName").value = cells[2].textContent.trim();
            document.getElementById("middleName").value = cells[3].textContent.trim();
            document.getElementById("lastName").value = cells[4].textContent.trim();
            document.getElementById("suffix").value = cells[5].textContent.trim();
            document.getElementById("address").value = cells[6].textContent.trim();
            document.getElementById("emailAddress").value = cells[7].textContent.trim();
            document.getElementById("mobileNumber").value = cells[8].textContent.trim();
            document.getElementById("status").value = cells[9].textContent.includes("A") ? "Active" : "Inactive";
            editButton.disabled = false;
        }
    });
}


editButton.addEventListener("click", async function () {
    if (editButton.textContent === "Edit") {
        // Reset form to prepare for a new entry
        EnableForm();
        _UserId.focus();
        _UserId.disabled = true;
        editButton.textContent = "Update";
        return;
    }

    if (editButton.textContent === "Update") {
        // Collect form data
        const userData = {
            UserId: document.getElementById("userId").value,
            MemberType: document.getElementById("memberType").value,
            FirstName: document.getElementById("firstName").value,
            MiddleName: document.getElementById("middleName").value,
            LastName: document.getElementById("lastName").value,
            Suffix: document.getElementById("suffix").value,
            Address: document.getElementById("address").value,
            EmailAddress: document.getElementById("emailAddress").value,
            MobileNumber: document.getElementById("mobileNumber").value,
            Status: document.getElementById("status").value,
        };

        try {
            const response = await fetch("/User/UpdateUserInformation", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(userData),
            });

            const data = await response.json();
            if (data.success) {
                alert(data.message);
                ClearData();
                DisableForm();
                loadUserData();
                editButton.textContent = "Edit";
            } else {
                alert(data.message);
            }
        } catch (error) {
            console.error("Error:", error);
            alert("An error occurred while saving.");
        }
    }
});