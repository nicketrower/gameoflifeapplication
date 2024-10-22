import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TopRowHeaderComponent } from './top-row-header-component';

describe('TopRowHeaderComponent', () => {
  let component: TopRowHeaderComponent;
  let fixture: ComponentFixture<TopRowHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        TopRowHeaderComponent
      ],
      declarations: [TopRowHeaderComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TopRowHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component (positive test)', () => {
    expect(component).toBeTruthy();
  });

});