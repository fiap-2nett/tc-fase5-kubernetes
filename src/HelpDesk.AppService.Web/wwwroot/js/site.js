// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

(function () {
    const ActionType = {
        Create: 1,
        Edit: 2,
        AssignTo: 3,
        AssignToMe: 4,
        ChangeStatusTo: 5,
        ChangeStatusToComplete: 6,
        Cancellation: 7
    };

    const modalAction = $('#modalActions');
    const toastPlacementExample = document.querySelector('.toast-placement-ex');
    const toastPlacementExampleTitle = document.querySelector('.toast-title');
    const toastPlacementExampleBody = document.querySelector('.toast-body');
    let toastType, toastPlacement;

    bindActionButtons = function () {
        let actionButtons = document.querySelectorAll('.hd-action-button');
        $('.hd-action-button').off('click').on('click', event => {
            event.preventDefault();

            const { ticketId, actionType } = event.target.dataset;
            const instantActions = [ActionType.AssignToMe, ActionType.ChangeStatusToComplete];

            if (instantActions.includes(Number(actionType))) {
                var payload = [
                    { name: 'IdTicket', value: ticketId },
                    { name: 'IdActionType', value: actionType }
                ];

                $.post(`/Ticket/DoActionTicket/${ticketId}`, payload)
                    .done(data => toastApply(data));
            }
            else {
                $.get(`/Ticket/GetActionModal?ticketId=${ticketId}&actionType=${actionType}`)
                    .done(data =>
                    {
                        if (!data.isSuccess) {
                            toastApply(data);
                            return;
                        }

                        modalAction.html(data.partialView);
                        modalAction.off('shown.bs.modal').on('shown.bs.modal', function () {
                            bindModalObjects(actionType);
                        }).modal('show');
                    });
            }
        });
    }

    const bindModalObjects = function (actionType) {
        let formAction = $('#formTicketAction');
        if (formAction.length > 0) {
            formAction.validate();

            if ([ActionType.Create, ActionType.Edit].includes(Number(actionType)))
            {
                $('#IdCategory').rules('add', { required: true });
                $('#Description').rules('add', { required: true });
            }
            else if (Number(actionType) == ActionType.AssignTo)
            {
                $('#IdUserAssigned').rules('add', { required: true });
            }
            else if (Number(actionType) == ActionType.ChangeStatusTo)
            {
                $('#IdStatusChanged').rules('add', { required: true });
            }
            else if (Number(actionType) == ActionType.Cancellation)
            {
                $('#CancellationReason').rules('add', { required: true });
            }
            
            formAction.off('submit').on('submit', event => {
                event.preventDefault();
                
                const form = event.target;
                const payload = $(form).serialize();

                const isValid = $(form).valid();
                if (!isValid) return;
                
                $.post(form.action, payload)
                    .done(data => toastApply(data));
            })
        }
    }

    const toastDispose = function (toast) {
        if (toast && toast._element !== null) {
            if (toastPlacementExample) {
                toastPlacementExample.classList.remove(toastType);                
            }

            toast.dispose();
        }
    }

    const toastApply = function (data) {
        if (toastPlacement) {
            toastDispose(toastPlacement);
        }

        toastType = data.isSuccess
            ? "bg-success"
            : "bg-danger";

        const toastTitle = data.isSuccess
            ? "Success!"
            : "Oops... an error occurred!";

        let toastBody = data.isSuccess
            ? "Success in processing your action."
            : "Error processing your action, see details:";

        if (!data.isSuccess) {
            toastBody += '<ul class="mt-3">';
            toastBody += data.errors.map((error) => `<li>${error.message}</li>`);
            toastBody += '</ul>';
        }

        toastPlacementExample.classList.add(toastType);
        toastPlacementExampleTitle.innerHTML = toastTitle;
        toastPlacementExampleBody.innerHTML = toastBody;        

        toastPlacement = new bootstrap.Toast(toastPlacementExample);        
        toastPlacement.show();

        if (data.isSuccess) {
            modalAction.modal('hide');
            reloadTickets();
        }
    }

    const reloadTickets = function () {        
        $.get('/Home/Tickets')
            .done(data => {
                $('#tableTickets tbody').html(data.partialView);
                bindActionButtons();
            });
    }

    bindActionButtons();
})();
