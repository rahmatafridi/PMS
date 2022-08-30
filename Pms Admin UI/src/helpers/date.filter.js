import dayjs from 'dayjs'

function dateFilter(date) {
  if (date) return dayjs(date).format('DD.MM.YYYY HH:mm')
}

function calendarDateFilter(date) {
  if (date) return dayjs(date).format('DD.MM.YYYY')
}

function timeFilter(date) {
  if (date) return dayjs(date).format('HH:mm')
}

export { calendarDateFilter, dateFilter, timeFilter }
