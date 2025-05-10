<template>
  <div class="report-view">
    <h1>Task Reports</h1>
    
    <div class="report-form">
      <h2>Generate New Report</h2>
      <form @submit.prevent="generateReport">
        <div class="form-group">
          <label for="startDate">Start Date</label>
          <input 
            type="date" 
            id="startDate" 
            v-model="reportParams.startDate"
            required
          />
        </div>
        
        <div class="form-group">
          <label for="endDate">End Date</label>
          <input 
            type="date" 
            id="endDate" 
            v-model="reportParams.endDate"
            required
          />
        </div>
        
        <div class="form-group">
          <label for="projectId">Project (Optional)</label>
          <select id="projectId" v-model="reportParams.projectId">
            <option :value="null">All Projects</option>
            <option v-for="project in projects" :key="project.id" :value="project.id">
              {{ project.name }}
            </option>
          </select>
        </div>
        
        <div class="form-group">
          <label for="statusFilter">Status Filter (Optional)</label>
          <select id="statusFilter" v-model="reportParams.statusFilter">
            <option :value="null">All Statuses</option>
            <option value="ToDo">To Do</option>
            <option value="InProgress">In Progress</option>
            <option value="InReview">In Review</option>
            <option value="Done">Done</option>
          </select>
        </div>
        
        <div class="form-group">
          <label for="priorityFilter">Priority Filter (Optional)</label>
          <select id="priorityFilter" v-model="reportParams.priorityFilter">
            <option :value="null">All Priorities</option>
            <option value="Low">Low</option>
            <option value="Medium">Medium</option>
            <option value="High">High</option>
            <option value="Critical">Critical</option>
          </select>
        </div>
        
        <div class="form-actions">
          <button type="submit" :disabled="isLoading">
            {{ isLoading ? 'Generating...' : 'Generate Report' }}
          </button>
        </div>
      </form>
    </div>
    
    <div v-if="currentReport" class="report-results">
      <h2>Report Results</h2>
      <div class="report-header">
        <h3>{{ currentReport.reportName }}</h3>
        <p>Generated: {{ new Date(currentReport.generatedDate).toLocaleString() }}</p>
      </div>
      
      <div class="report-stats">
        <div class="stat-card">
          <div class="stat-value">{{ currentReport.totalTasks }}</div>
          <div class="stat-label">Total Tasks</div>
        </div>
        <div class="stat-card">
          <div class="stat-value">{{ currentReport.completedTasks }}</div>
          <div class="stat-label">Completed</div>
        </div>
        <div class="stat-card">
          <div class="stat-value">{{ currentReport.inProgressTasks }}</div>
          <div class="stat-label">In Progress</div>
        </div>
        <div class="stat-card">
          <div class="stat-value">{{ currentReport.pendingTasks }}</div>
          <div class="stat-label">Pending</div>
        </div>
        <div class="stat-card">
          <div class="stat-value">{{ Math.round(currentReport.completionRate) }}%</div>
          <div class="stat-label">Completion Rate</div>
        </div>
      </div>
      
      <div class="report-actions">
        <a :href="currentReport.reportUrl" target="_blank" class="download-btn">
          Download {{ currentReport.exportFormat }}
        </a>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue';
import { generateTaskReport, fetchProjects } from '@/services/api';

const reportParams = reactive({
  startDate: new Date(new Date().setDate(new Date().getDate() - 30)).toISOString().split('T')[0],
  endDate: new Date().toISOString().split('T')[0],
  projectId: null,
  statusFilter: null,
  priorityFilter: null
});

const projects = ref([]);
const currentReport = ref(null);
const isLoading = ref(false);

onMounted(async () => {
  try {
    const response = await fetchProjects();
    projects.value = response.data;
  } catch (error) {
    console.error('Failed to fetch projects:', error);
  }
});

const generateReport = async () => {
  try {
    isLoading.value = true;
    
    // Prepare dates in the format expected by the API
    const params = {
      ...reportParams,
      startDate: new Date(reportParams.startDate).toISOString(),
      endDate: new Date(reportParams.endDate).toISOString()
    };
    
    const response = await generateTaskReport(params);
    currentReport.value = response.data;
  } catch (error) {
    console.error('Failed to generate report:', error);
    alert('Failed to generate report. Please try again.');
  } finally {
    isLoading.value = false;
  }
};
</script>

<style scoped>
.report-view {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

h1 {
  margin-bottom: 25px;
  color: #333;
}

.report-form {
  background-color: #f8f9fa;
  padding: 20px;
  border-radius: 8px;
  margin-bottom: 30px;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: 500;
}

.form-group input,
.form-group select {
  width: 100%;
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 16px;
}

.form-actions {
  margin-top: 20px;
}

.form-actions button {
  background-color: #4CAF50;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
}

.form-actions button:disabled {
  background-color: #cccccc;
  cursor: not-allowed;
}

.report-results {
  background-color: #fff;
  border: 1px solid #eee;
  border-radius: 8px;
  padding: 20px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

.report-header {
  margin-bottom: 20px;
  border-bottom: 1px solid #eee;
  padding-bottom: 10px;
}

.report-stats {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
  gap: 15px;
  margin-bottom: 30px;
}

.stat-card {
  background-color: #f8f9fa;
  border-radius: 8px;
  padding: 15px;
  text-align: center;
}

.stat-value {
  font-size: 24px;
  font-weight: bold;
  color: #333;
  margin-bottom: 5px;
}

.stat-label {
  color: #666;
  font-size: 14px;
}

.report-actions {
  margin-top: 20px;
  text-align: center;
}

.download-btn {
  display: inline-block;
  background-color: #2196F3;
  color: white;
  text-decoration: none;
  padding: 10px 20px;
  border-radius: 4px;
  font-weight: 500;
}

.download-btn:hover {
  background-color: #0b7dda;
}
</style>