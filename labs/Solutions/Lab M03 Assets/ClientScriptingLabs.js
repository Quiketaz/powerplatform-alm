CalculateLeadTime = function (executionContext) {
    var formContext = executionContext.getFormContext();
    var dueDate = formContext.getAttribute("scheduledend");
    var scheduledStart = formContext.getAttribute("scheduledstart");
    if (!dueDate || !scheduledStart || !dueDate.getValue() ||
        !scheduledStart.getValue())
        return;
    var dayAsMilliseconds = 86400000;
    var leadTimeDays = Math.ceil((dueDate.getValue().getTime() -
        scheduledStart.getValue().getTime()) / dayAsMilliseconds);
    var leadTimeDaysAttribute = formContext.getAttribute("pfedyn_leadtimedays");
    leadTimeDaysAttribute.setValue(leadTimeDays);
}


ControlSectionDisplay = function (executionContext, attributeName, sectionName) {
    var isVisible = false;
    var formContext = executionContext.getFormContext();
    var attribute = formContext.getAttribute(attributeName);
    if (!attribute) {
        Xrm.Navigation.openAlertDialog({
            text: `Attribute '${attributeName}'not found on form. Check event handler configuration`,
            title: "Configuration error", confirmButtonLabel: "OK"
        });
        return;
    }

    isVisible = attribute.getValue() !== null;

    formContext.ui.tabs.forEach(function (tab, index) {
        tab.sections.forEach(function (section, index) {
            var sectionNameMatch = section.getLabel().indexOf(sectionName, 0) !== -1;
            if (sectionNameMatch) {
                if (!isVisible) {
                    section.controls.forEach(function (control, index) {
                        control.getAttribute().setValue(null);
                    });
                }
                section.setVisible(isVisible);
            }
        });
    });
}
