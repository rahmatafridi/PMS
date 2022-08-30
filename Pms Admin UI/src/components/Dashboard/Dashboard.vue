<template>
  <div>
    <!--Stats cards-->
    <div class="row">
      <div class="col-lg-3 col-sm-6">
        <stats-card>
          <div class="icon-big text-center ti-server" slot="header">
            <i class="stats.icon"></i>
          </div>
          <div slot="content">

            <p>assigned :  {{assignedProperty}}</p>
            <p>occupancy : {{occupancyProperty}}%</p>

          </div>
          <div class="stats" slot="footer">
            <h6 v-on:click="goToProperty">Properties : {{totalProperty}}</h6>
          </div>
        </stats-card>
      </div>
      <div class="col-lg-3 col-sm-6">
        <stats-card>
          <div class="icon-big text-center ti-home" slot="header">
            <i class="stats.icon"></i>
          </div>
          <div slot="content">

            <p>assigned :  {{assignedRoom}}</p>
            <p>occupancy : {{occupancyRoom}}%</p>

          </div>
          <div class="stats" slot="footer">
            <h6 v-on:click="goToRoom">
              Rooms :  {{totalRoom}}
            </h6>
          </div>
        </stats-card>
      </div>
      <div class="col-lg-3 col-sm-6">
        <stats-card>
          <div class="icon-big text-center ti-user" slot="header">
            <i class="stats.icon"></i>
          </div>
          <div slot="content">

            <p>assigned :  {{assignedTenant}}</p>
            <p>occupancy : {{occupancyTenant}}%</p>

          </div>
          <div class="stats" slot="footer">
            <h6 v-on:click="goToTenant">Tenants :  {{totalTenant}}</h6>
          </div>
        </stats-card>
      </div>
    </div>

    <div class="row">
      <div class="col-lg-3 col-sm-6">
        <stats-card>
          <div class="icon-big text-center ti-write" slot="header">
            <i class="stats.icon"></i>
          </div>
          <div slot="content">

            <p> {{missingDoc}}</p>
            <p v-on:click="goToMissingDoc">Missing Documents</p>

          </div>

        </stats-card>
      </div>
      <div class="col-lg-3 col-sm-6">
        <stats-card>
          <div class="icon-big text-center ti-close" slot="header">
            <i class="stats.icon"></i>
          </div>
          <div slot="content">

            <p> {{expired}}</p>
            <p v-on:click="goToExpiredDoc">Expired</p>

          </div>

        </stats-card>
      </div>
      <div class="col-lg-3 col-sm-6">
        <stats-card>
          <div class="icon-big text-center ti-info-alt" slot="header">
            <i class="stats.icon"></i>
          </div>
          <div slot="content">
            <p> {{expiringIn7Day}}</p>
            <p v-on:click="goToExpiredDoc">Expiring in 7 Days</p>


          </div>
        </stats-card>
      </div>
      <div class="col-lg-3 col-sm-6">
        <stats-card>
          <div class="icon-big text-center ti-time" slot="header">
            <i class="stats.icon"></i>
          </div>
          <div slot="content">
            <p> {{expiringIn28Day}}</p>
            <p v-on:click="goToExpiredDoc">Expiring in 28 Days</p>


          </div>
        </stats-card>
      </div>

    </div>


  </div>
</template>
<script>
  import CircleChartCard from 'components/UIComponents/Cards/CircleChartCard.vue'
  import StatsCard from 'src/components/UIComponents/Cards/StatsCard.vue'
  import { getDataa } from '@/helpers/dashboard'

  export default {
    components: {
      StatsCard,
      CircleChartCard
    },
   
    data () {
      return {
        totalProperty: '',
        assignedProperty: '',
        occupancyProperty: '',

        totalTenant: '',
        assignedTenant: '',
        occupancyTenant: '',

        totalRoom: '',
        assignedRoom: '',
        occupancyRoom: '',

        missingDoc: '',
        expired: '',
        expiringIn7Day: '',
        expiringIn28Day:''


      }
    },
   
    methods: {
      getDataa: getDataa,
      loadData() {
        this.getDataa().then(res => {
          console.log(res);
          this.totalProperty = res.totalProperty;
          this.assignedProperty = res.assignedProperty;
          this.occupancyProperty = res.occupancyProperty;

          this.totalTenant = res.totalTenant;
          this.assignedTenant = res.assignedTenant;
          this.occupancyTenant = res.occupancyTenant;

          this.totalRoom = res.totalRoom;
          this.assignedRoom = res.assignedRoom;
          this.occupancyRoom = res.occupancyRoom;

          this.missingDoc = res.missingDoc;
          this.expired = res.expiredDoc;
          this.expiringIn7Day = res.expiredDoc7;
          this.expiringIn28Day = res.expiredDoc28;
        })

      },
      goToProperty() {
         this.$router.push('/property')
      },
      goToRoom() {
        this.$router.push('/emptyroom')
      },
      goToTenant() {
        this.$router.push('/tenantreport')
      },
      goToMissingDoc() {
        this.$router.push('/missingdoc')

      },
      goToExpiredDoc() {
        this.$router.push('/docexpiry')

      }
    },
    mounted() {
      this.loadData();
    },
  }</script>
<style>
</style>
