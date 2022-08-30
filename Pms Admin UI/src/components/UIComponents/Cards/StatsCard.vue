<template>
  <div class="card">
    <div class="card-content">
      <div class="row" style="margin-top:10px;">
        <div class="col-xs-4">
          <slot name="header"></slot>
        </div>
        <div class="col-xs-8">
          <slot name="content"></slot>
        </div>
      </div>
    </div>
    <div class="card-footer">
      <hr/>
      <slot name="footer"></slot>
    </div>

  </div>
</template>
<script>
  export default {
    name: 'stats-card'
  }

</script>
<style scoped>
  .card-content{
    min-height: 85px
  }
</style>
