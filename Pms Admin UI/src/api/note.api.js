import ApiService from './api.service'

const apiService = new ApiService('', true)
const NoteApi = {
  note: {
    get(params) {
      return apiService.query('/v1/note/getnotebyid', params)
    },
    getParent(params) {
      return apiService.query('/v1/note/getnotebyperantid', params)
    },
    list(params) {
      return apiService.query('/v1/note/getnote', params)
    },
    add(addNote) {
      return apiService.post('/v1/note/add', addNote)
    },
    update(updateNote) {
      return apiService.put('/v1/note/update', updateNote)
    },
    delete(noteId) {
      return apiService.deleteWithParames('/v1/note/delete', { noteId })
    },

  }
}

export default NoteApi
