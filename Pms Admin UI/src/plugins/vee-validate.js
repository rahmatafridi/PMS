import Vue from 'vue'
import {
  extend,
  ValidationObserver,
  ValidationProvider,
} from 'vee-validate'
import {
  required, digits, email, min, max, regex, numeric
} from 'vee-validate/dist/rules'
import { validateClientEmail } from '@/helpers/client'
import { validateRoleNameByClient } from '@/helpers/role'
import { validateUserEmail, validateUserName } from '@/helpers/user'
import { validateProviderEmail } from '@/helpers/provider'

extend('numeric', {
  ...numeric,
  message: '{_field_} needs to be numeric.',
})

extend('digits', {
  ...digits,
  message: '{_field_} needs to be {length} digits. ({_value_})',
})

extend('required', {
  ...required,
  message: '{_field_} is required',
})

extend('min', {
  ...min,
  message: '{_field_} should be greater than {length} characters',
})

extend('max', {
  ...max,
  message: '{_field_} may not be greater than {length} characters',
})

extend('regex', {
  ...regex,
  message: '{_field_} {_value_} does not match {regex}',
})

extend('email', {
  ...email,
  message: 'Email must be valid',
})

extend('isClientEmailExists', {
  params: ['id'],
  async validate(value, { id }) {
    let errorMessage = true
    const client = {
      id: id,
      email: value,
    }
    await validateClientEmail(client)
      .then(res => {
        if (res)
          errorMessage = true
      })
      .catch(e => {
        if (e == 'Email is already in use.') {
          errorMessage = false
        } else
          errorMessage = true
      })
    return errorMessage
  },
  message: '{_field_} is already in use',
})

extend('isRoleNameExists', {
  params: ['id', 'clientId'],
  async validate(value, { id, clientId }) {
    let errorMessage = true
    const role = {
      id: id,
      clientId: clientId,
      name: value,
    }
    await validateRoleNameByClient(role)
      .then(res => {
        if (res)
          errorMessage = true
      })
      .catch(e => {
        if (e == 'rolename is already in use.') {
          errorMessage = false
        } else
          errorMessage = true
      })
    return errorMessage
  },
  message: '{_field_} is already in use',
})

extend('isUserEmailExists', {
  params: ['id'],
  async validate(value, { id }) {
    let errorMessage = true
    const user = {
      id: id,
      email: value,
    }
    await validateUserEmail(user)
      .then(res => {
        if (res)
          errorMessage = true
      })
      .catch(e => {
        if (e == 'Email is already in use.') {
          errorMessage = false
        } else
          errorMessage = true
      })
    return errorMessage
  },
  message: '{_field_} is already in use',
})

extend('isUserNameExists', {
  params: ['id'],
  async validate(value, { id }) {
    let errorMessage = true
    const user = {
      id: id,
      userName: value,
    }
    await validateUserName(user)
      .then(res => {
        if (res)
          errorMessage = true
      })
      .catch(e => {
        if (e == 'Username is already in use.') {
          errorMessage = false
        } else
          errorMessage = true
      })
    return errorMessage
  },
  message: '{_field_} is already in use',
})

extend('isProviderEmailExists', {
  params: ['id'],
  async validate(value, { id }) {
    let errorMessage = true
    const provider = {
      id: id,
      email: value,
    }
    await validateProviderEmail(provider)
      .then(res => {
        if (res)
          errorMessage = true
      })
      .catch(e => {
        if (e == 'Email is already in use.') {
          errorMessage = false
        } else
          errorMessage = true
      })
    return errorMessage
  },
  message: '{_field_} is already in use',
})

Vue.component('validation-provider', ValidationProvider)
Vue.component('validation-observer', ValidationObserver)
