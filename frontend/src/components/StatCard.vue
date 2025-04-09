<template>
  <div class="stat-card" :class="data.type">
    <div class="stat-icon">
      <i :class="'icon-' + data.icon"></i>
    </div>
    <div class="stat-content">
      <h3 class="stat-value">{{ data.value }}</h3>
      <p class="stat-title">{{ data.title }}</p>
    </div>
  </div>
</template>

<script setup>
import { defineProps } from 'vue';

const props = defineProps({
  data: {
    type: Object,
    required: true,
  },
});
</script>

<style scoped>
.stat-card {
  background-color: #ffffff;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  padding: 20px;
  display: flex;
  align-items: center;
  min-width: 200px;
}

.stat-card.primary { border-left: 4px solid #4caf50; }
.stat-card.warning { border-left: 4px solid #ff9800; }
.stat-card.danger { border-left: 4px solid #f44336; }
.stat-card.info { border-left: 4px solid #2196f3; }

.stat-icon {
  margin-right: 15px;
  font-size: 24px;
  color: #757575;
}

.stat-value {
  font-size: 24px;
  margin: 0 0 5px 0;
}

.stat-title {
  color: #757575;
  margin: 0;
  font-size: 14px;
}
</style>