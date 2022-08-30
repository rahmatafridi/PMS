import ApiService from './api.service'

const apiService = new ApiService('', true)
const DocumentApi = {
  document: {
    get(documentId) {
      return apiService.query('/v1/document/getdocumentbyid', { documentId })
    },
    list(params) {
      return apiService.query('/v1/document/getdocuments', params)
    },
    add(addNote) {

      return apiService.post('/v1/document/add', addNote)
    },
    update(updateNote) {
      return apiService.put('/v1/document/update', updateNote)
    },
    delete(noteId) {
      return apiService.deleteWithParames('/v1/document/delete', { noteId })
    },

  }
}

export default DocumentApi
