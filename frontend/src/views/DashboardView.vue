<template>
  <div class="dashboard">
    <h1>{{ title }}</h1>
    <div class="stats-container">
      <stat-card v-for="(stat, index) in stats" :key="index" :data="stat" />
    </div>
    <data-table :items="items" :columns="columns" @row-click="handleRowClick" />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import StatCard from '@/components/StatCard.vue';
import DataTable from '@/components/DataTable.vue';
import { fetchDashboardData } from '@/services/api';

const title = ref('Dashboard');
const stats = ref([]);
const items = ref([]);
const columns = ref([
  { field: 'id', label: 'ID' },
  { field: 'name', label: 'Name' },
  { field: 'status', label: 'Status' }
]);

onMounted(async () => {
  try {
    const data = await fetchDashboardData();
    stats.value = data.stats;
    items.value = data.items;
  } catch (error) {
    console.error('Failed to fetch dashboard data:', error);
  }
});

const handleRowClick = (item) => {
  console.log('Row clicked:', item);
};
</script>

<style scoped>
.dashboard {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.stats-container {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
  gap: 20px;
  margin-bottom: 30px;
}

h1 {
  margin-bottom: 25px;
  color: #333;
}
</style>