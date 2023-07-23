function confirmDelete(uniqueId, isDeleteClicked) {
  var deleteSpan = 'DeleteSpan_' + uniqueId;
  var confirmDeleteSpan = 'confirDeleteSpan_' + uniqueId;

  if (isDeleteClicked) {
    $('#' + deleteSpan).hide();
    $('#' + confirmDeleteSpan).show();
  } else {
    $('#' + deleteSpan).show();
    $('#' + confirmDeleteSpan).hide();
  }
}
