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