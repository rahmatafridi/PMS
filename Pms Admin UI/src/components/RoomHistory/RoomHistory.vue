<style>
  card .card-title, .card .stats, .card .category, .card .description, .card .social-line, .card .actions, .card .card-content, .card .card-footer, .card small, .card a {
    position: initial !important;
  }
</style>
<template>
  <div class="row">
    <div class="col-md-12">
      <div class="">

        <div class="row">
          <div v-for="item in roomHistory">
            <div class="card-content">
              <div class="col-md-12">
                <span style="float:left"><label> Room:</label> {{item.roomNo}}</span>
              </div>
              <div class="col-md-12">
                <span style="float:left"><label> Property:</label> {{item.property}}</span>
              </div>
              <div class="col-md-12">
                <span style="float:left"><label> Tenancy Start date:</label> {{item.tenancyStartDate | formatDate}}</span>
              </div>
              <div class="col-md-12">
                <span style="float:left"> <label> Tenancy End date:</label> {{item.tenancyEndDate | formatDate}}</span>
              </div>
              <div class="col-md-12" style="margin-bottom:20px;">
                <span style="float:left" v-if="item.isTenantIsMoving === true"><label> Reason: </label> Tenant has been Shift</span>
                <span style="float:left" v-if="item.isTenantLeaving === true"> <label> Reason: </label> Tenant has been Left</span>
              </div>
            </div>


          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>

  import { getRoomHistoryById } from '@/helpers/roomhistory'
  import swal from 'sweetalert2'


  export default {
    components: {
     
    },
    data() {
      return {
        isLoading: false,
        IsValid:false,
        errorMessage: null,
        roomHistory: [],
 
        
      }
    },
    mounted() {
     // let id = this.$route.params.id;
      let str = this.$route.params.id;
      var dId = this.aesDecript1(str);
      var id = parseInt(dId.slice(0, -1));
      if (id != null) {
        this.getRoomHistoryById(id)
          .then(res => {

            this.roomHistory = res.items;
          })
          .catch(ex => {
            this.errorMessage = ex
            swal({
              title: 'Warning',
              text: this.errorMessage,
              type: 'warning',
              confirmButtonClass: 'btn btn-success btn-fill',
              buttonsStyling: false
            })          })
      }
    },
    methods: {
      getRoomHistoryById: getRoomHistoryById,

    },
   

  }
</script>
