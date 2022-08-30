import ApiService from './api.service'

const apiService = new ApiService('', true)

const PropertyApi = {
  property: {
    get(propertyId) {
      return apiService.query('/v1/property/getpropertybyid', { propertyId })
    },
    list(params) {
      return apiService.query('/v1/property/getproperties', params)
    },
    add(addProperty) {
      return apiService.post('/v1/property/add', addProperty)
    },
    update(updateProperty) {
      return apiService.put('/v1/property/update', updateProperty)
    },
    delete(propertyId) {
      return apiService.deleteWithParames('/v1/property/delete', { propertyId })
    },
  
  }
}

export default PropertyApi
