import axios from 'axios'
//import router from '../routes/routes'
import authService from '@/services/auth.service'
var ApiUrl = "http://localhost:51944"

class ApiService {
  
  constructor(apiUrl, includeToken) {
    this.axios = axios.create({
      baseURL: ApiUrl,
      headers: { 'content-type': 'application/json' }
    })

    this.includeToken = includeToken

    axios.interceptors.request.use(
      config => {
        if (includeToken) {
          const token = authService.getToken()
          if (token) {
            config.headers['Authorization'] = 'Bearer ' + token
          }
        }

        return config
      },
      error => {
        Promise.reject(error)
      }
    )

    this.axios.interceptors.request.use(
      config => {
        if (includeToken) {
          const token = authService.getToken()
          if (token) {
            config.headers['Authorization'] = 'Bearer ' + token
          }
        }

        return config
      },
      error => {
        Promise.reject(error)
      }
    )

    this.axios.interceptors.response.use(
      response => {
        return { ok: true, data: response.data }
      },
      error => {
        if (error.response && error.response.status === 401) {
          router.push('/login')
          return Promise.reject(error)
        }

        return { ok: false, error: error }
      }
    )
  }

  query (resource, params) {
    return this.axios.get(resource, { params: params })
  }

  get (resource, slug = '') {
    return this.axios.get(`${resource}/${slug}`)
  }

  post (resource, data, config = null) {
    return this.axios.post(`${resource}`, data, config)
  }

  put (resource, data, slug = '') {
    return this.axios.put(`${resource}/${slug}`, data)
  }

  delete (resource, slug = '') {
    return this.axios.delete(`${resource}/${slug}`)
  }
  deleteWithParames (resource, params = {}) {
    return this.axios.delete(`${resource}`, { params: params })
  }

  form (resource, form) {
    return this.axios.post(resource, form, {
      headers: { 'content-type': 'multipart/form-data' }
    })
  }
}

export default ApiService
