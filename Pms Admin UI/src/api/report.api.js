import ApiService from './api.service'

const apiService = new ApiService('', true);
const ReportApi = {
  report: {
    tenantReport(param) {
      return apiService.query('/v1/report/gettenantreport', param)
    },
    emptyRoomReport() {
      return apiService.query('/v1/report/emptyroomsreport')
    }
    ,
    missingDocReport(param) {
      return apiService.query('/v1/report/getmissingdocreport', param)

    },
    expiryDocReport(param) {
      return apiService.query('/v1/report/getexpirydocreport', param)

    }
  }
}

export default ReportApi

